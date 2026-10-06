namespace SmartLighting.Web.Features.Devices.Contracts.ViewModels;

public sealed class CreateDeviceFormVM
{
    public string Name { get; set; } = string.Empty;

    public IReadOnlyCollection<DeviceSensorSlotFormVM> Sensors { get; } =
    [
        new(1, 32, "Sensor 1"),
        new(2, 34, "Sensor 2"),
        new(3, 35, "Sensor 3")
    ];

    public IReadOnlyCollection<DeviceLuminaireSlotFormVM> Luminaires { get; } =
    [
        new(1, 18, "LED 1"),
        new(2, 19, "LED 2"),
        new(3, 21, "LED 3"),
        new(4, 25, "LED 4"),
        new(5, 26, "LED 5"),
        new(6, 27, "LED 6")
    ];
}

public sealed class DeviceSensorSlotFormVM(int slot, int gpioNumber, string name)
{
    public int Slot { get; } = slot;
    public int GpioNumber { get; } = gpioNumber;
    public bool Enabled { get; set; }
    public bool IsPersisted { get; set; }
    public string Name { get; set; } = name;
    public Guid? ZoneId { get; set; }
}

public sealed class DeviceLuminaireSlotFormVM(int slot, int gpioNumber, string name)
{
    public int Slot { get; } = slot;
    public int GpioNumber { get; } = gpioNumber;
    public bool Enabled { get; set; }
    public bool IsPersisted { get; set; }
    public string Name { get; set; } = name;
    public Guid? ZoneId { get; set; }
    public decimal? NominalPowerWatts { get; set; }
}
