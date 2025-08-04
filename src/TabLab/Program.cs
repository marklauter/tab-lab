using TabLab;

Console.WriteLine("Welcome to TabLab - The ASCII Guitar Tab Player!");

// happy birthday: tab credit : https://tabs.ultimate-guitar.com/tab/misc-traditional/happy-birthday-tabs-42938
// 3/4 time
var tab = """
                  e|------------|------------|--------------|-------------|
                  B|-0-0-2-0-5--|-0-0-2-0----|-0-0-----5---2|-------------|
                  G|------------|------------|-----4-1------|-2-2-1-------|
                  D|---------2-1|---------4-2|---------2-1--|-------2-4-2-|
                  A|-2-2-4-2---9|-2-2-4-2----|-2-2-------9-4|-------------|
                  E|---------0--|---------2-0|---------0----|-------0-2-0-|                  
                  """;

var moments = Parser.Parse(tab);
Console.WriteLine($"Parsed {moments.Length} moments from the tab.");

await Player.PlayMoments(moments);

Console.WriteLine("Playback finished. Press any key to exit.");

_ = Console.ReadKey();