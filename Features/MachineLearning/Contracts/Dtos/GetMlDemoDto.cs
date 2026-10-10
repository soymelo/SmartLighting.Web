namespace SmartLighting.Web.Features.MachineLearning.Contracts.Dtos;

public sealed record GetMlDemoDto(
    string Source,
    string ModelVersion,
    GetMlDemoDatasetDto Dataset,
    IReadOnlyCollection<GetMlDemoMetricDto> Metrics,
    IReadOnlyCollection<GetMlDemoExampleDto> Examples);

public sealed record GetMlDemoDatasetDto(
    int TrainingWindows,
    int ValidationWindows,
    int TestWindows,
    int SensorCount);

public sealed record GetMlDemoMetricDto(
    string Detector,
    int TruePositives,
    int FalsePositives,
    int TrueNegatives,
    int FalseNegatives);

public sealed record GetMlDemoExampleDto(
    string SensorId,
    DateTime WindowStartUtc,
    int EventCount5m,
    double AnomalyScore,
    bool IsAnomaly,
    bool ExpectedAnomaly,
    string Selection);
