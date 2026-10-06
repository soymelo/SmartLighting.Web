using SmartLighting.Web.Features.Devices.Contracts.ViewModels;

namespace SmartLighting.Web.Features.Devices.Application;

public static class DeviceFormValidator
{
    public static bool Validate(CreateDeviceFormVM form, IDictionary<string, string> validationErrors)
    {
        validationErrors.Clear();

        if (string.IsNullOrWhiteSpace(form.Name))
        {
            validationErrors["name"] = "Informe o nome do dispositivo.";
        }
        else if (form.Name.Trim().Length > 50)
        {
            validationErrors["name"] = "O nome deve ter no máximo 50 caracteres.";
        }

        foreach (var sensor in form.Sensors.Where(sensor => sensor.Enabled))
        {
            ValidateComponentName(validationErrors, sensor.Name, $"sensor-{sensor.Slot}-name");

            if (sensor.ZoneId is null)
            {
                validationErrors[$"sensor-{sensor.Slot}-zone"] = "Selecione a zona do sensor.";
            }
        }

        foreach (var luminaire in form.Luminaires.Where(luminaire => luminaire.Enabled))
        {
            ValidateComponentName(validationErrors, luminaire.Name, $"luminaire-{luminaire.Slot}-name");

            if (luminaire.ZoneId is null)
            {
                validationErrors[$"luminaire-{luminaire.Slot}-zone"] = "Selecione a zona da luminária.";
            }

            if (luminaire.NominalPowerWatts is null or <= 0)
            {
                validationErrors[$"luminaire-{luminaire.Slot}-power"] = "Informe uma potência maior que zero.";
            }
            else if (luminaire.NominalPowerWatts > 99999999.99m ||
                     decimal.Round(luminaire.NominalPowerWatts.Value, 2) != luminaire.NominalPowerWatts.Value)
            {
                validationErrors[$"luminaire-{luminaire.Slot}-power"] = "Use até 8 dígitos inteiros e 2 casas decimais.";
            }
        }

        return validationErrors.Count == 0;
    }

    private static void ValidateComponentName(IDictionary<string, string> validationErrors, string name, string key)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            validationErrors[key] = "Informe o nome do componente.";
        }
        else if (name.Trim().Length > 50)
        {
            validationErrors[key] = "O nome deve ter no máximo 50 caracteres.";
        }
    }
}
