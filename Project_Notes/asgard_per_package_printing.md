# Asgard Labels (Acumatica 2024)
## Per-Package Printing via Custom Button – Technical Write-Up

---

## Objective

Enable **printing one label per selected package** from `SOShipmentEntry` using a custom button, while:

- Preserving **native Asgard printing behavior**
- Using:
  - `LabelContext.CreatePrintContext(...)`
  - `BasicLabelGenerator.PrintLabels(...)`
  - `PXLongOperation`
- Avoiding:
  - row-shape errors
  - cache/type issues
  - breaking Asgard’s internal pipeline

---

## Core Insight

> Do NOT try to pass a single package row into Asgard.  
> Instead: Let Asgard resolve its view normally, but control what that view returns.

---

## Asgard Printing Pipeline

```
CreatePrintContext(...)
    ↓
BasicLabelGenerator.PrintLabels(...)
    ↓
PrintLabelInternal(...)
    ↓
ViewUtils.GetViewRow(...)
    ↓
ViewResult
    ↓
ViewUtils.ViewSelect(graph, BasedOnView)
    ↓
PXView.Select()
```

Asgard always pulls data from the model’s `BasedOnView`.

---

## Why Earlier Approaches Failed

### Attempt 1: `CreateSingleRowPrintContext(...)` with `SOPackageDetail`
- Fails in `PXResult.UnwrapMain(...)`
- Reason: model expects joined result rows

### Attempt 2: Passing `PXResult`
- Fails in cache resolution
- Reason: `PXResult` is not a valid cache type

### Attempt 3: Filtering `DetailRows`
- Unreliable or null
- Depends on model iterator configuration

---

## Final Working Strategy

Use:
```
LabelContext.CreatePrintContext(...)
```

Do NOT use:
```
CreateSingleRowPrintContext(...)
```

---

## The Trick

Override the `ALPackages` view temporarily during printing.

Instead of:
```
ALPackages → ALL packages
```

Force:
```
ALPackages → ONLY selected packages
```

---

## Why This Works

- `BasicLabelGenerator` uses `viewRow.Result`
- `viewRow.Result` comes from:
```
ViewUtils.ViewSelect(graph, "ALPackages")
```

By filtering `ALPackages.Select()`, Asgard naturally prints only selected rows.

---

## Critical Files

### 1. BasicLabelGenerator.cs
- Controls print pipeline and row handling

### 2. ViewResult.cs
```
Result = ViewUtils.ViewSelect(graph, viewName)
```

### 3. ViewUtils.cs
- Resolves views and executes them

### 4. ALSOShipmentEntryExt.cs
```
PXSelectJoin<SOPackageDetail,...> ALPackages;
```

Only filters by shipment, not package.

### 5. LabelContext.cs
- Explains difference between print context methods

---

## Final Architecture

```
User selects packages
    ↓
PXLongOperation
    ↓
Activate filter scope
    ↓
CreatePrintContext(...)
    ↓
Asgard resolves ALPackages
    ↓
Filtered view returns selected packages
    ↓
PrintLabels(...)
```

---

## Implementation Components

### 1. AsyncLocal Filter Scope
Stores:
- ShipmentNbr
- Selected package line numbers

### 2. Graph Extension Override
```
Base.Views["ALPackages"] = new PXView(..., FilteredDelegate)
```

### 3. Filter Logic
```
if (FilterScope.Active)
    return selected packages
else
    return all packages
```

### 4. Service Layer
- Gather selected packages
- Activate filter
- Call print

---

## Constraints

- Must use same graph instance
- Must wrap filter in `using` block
- Do NOT modify model or inject fake rows

---

## Final Result

- One label per selected package
- Native Asgard behavior preserved
- No errors
- Scales well

---

## One-Line Summary

We achieved per-package printing by filtering the `ALPackages` view during `CreatePrintContext(...)`, allowing Asgard to process only selected packages naturally.

---

## Reusable Prompt

```
I am working in Acumatica 2024 with Asgard Labels.

I implemented per-package printing by overriding the ALPackages view during CreatePrintContext(...) so that ViewUtils.ViewSelect(...) only returns selected package rows.

Please assume:
- I am NOT using CreateSingleRowPrintContext
- I rely on BasicLabelGenerator’s native pipeline
- ALPackages is a PXSelectJoin<SOPackageDetail,...>
- I filter it via PXView delegate using AsyncLocal scope

Help me extend or debug this approach.
```
