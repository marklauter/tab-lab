using TabLab;

Console.WriteLine("Welcome to TabLab - The ASCII Guitar Tab Player!");

// happy birthday: tab credit : https://tabs.ultimate-guitar.com/tab/misc-traditional/happy-birthday-tabs-42938
var tab = """
                  e|-------------|-------------|---------------|-------------|
                  B|-------------|-------------|---------------|-------------|
                  G|-------------|-------------|-----4-1-------|-2-2-1-------|
                  D|---------2-1-|---------4-2-|---------2-1---|-------2-4-2-|
                  A|-2-2-4-2-----|-2-2-4-2-----|-2-2---------4-|-------------|
                  E|-------------|-------------|---------------|-------------|
                  """;

var moments = TabPlayer.ParseTab(tab);
Console.WriteLine($"Parsed {moments.Count} moments from the tab.");

await TabPlayer.PlayMoments(moments);

Console.WriteLine("Playback finished. Press any key to exit.");

_ = Console.ReadKey();