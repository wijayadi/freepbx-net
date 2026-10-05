# Call detail records

Call detail records are read from the CDR database through GraphQL.

```csharp
// Last 7 days (default), first 100 records.
var cdrs = await client.Cdrs.GetAsync();

// Explicit range and page size.
var march = await client.Cdrs.GetAsync(
    startDate: new DateTime(2026, 3, 1),
    endDate: new DateTime(2026, 3, 31),
    first: 500,
    orderBy: CdrOrderBy.Date);

foreach (var cdr in march)
{
    Console.WriteLine($"{cdr.CallDate} {cdr.Source} -> {cdr.Destination} [{cdr.Disposition}] {cdr.Duration}s");
}
```

## Single record

```csharp
var cdr = await client.Cdrs.GetByIdAsync("some-id");
```

## Notes

- `startDate` and `endDate` default to the last 7 days. FreePBX requires a date
  range for CDR queries.
- `first` defaults to `100`.
- `orderBy` is either `CdrOrderBy.Date` or `CdrOrderBy.Duration`.

## Model

`CdrDto` mirrors the FreePBX `cdr` type, including `CallDate`, `Source`,
`Destination`, `Disposition`, `Duration`, `BillableSeconds`, `RecordingFile`,
`UniqueId`, `LinkedId`, and the caller name/number fields.
