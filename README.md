
# Async Process
## Run a process asyncronously and get the output console in the result


````
var process = new AsyncProcess("path_to_your_exe", CancellationToken.None, param1, param2, paramn);

var ouput= await process.GetStringAsync();



var outputFromErrorStandardOutputOnly=  await process.GetStringAsync(flase, true);

var outputFromStandardOutputOnly=  await process.GetStringAsync(true, false);

var outputFromStandardAndErrorOutput=  await process.GetStringAsync(true, true); equaivalent to await process.GetStringAsync();


````
