if (args.Length == 0 || args[0] == "-h")
{
	Console.WriteLine("使用方法：");
	Console.WriteLine("[本可执行文件] [要被处理的 txt 文件] [输出文件名（没有此参数则会使用默认名称）]");
	Console.WriteLine("默认名称为输入文件的名称后面加上输出两个字，即：[要被处理的 txt 文件名]输出");
	return;
}

string in_file_name = args[0];
string out_file_name;
if (args.Length >= 2)
{
	out_file_name = args[1];
}
else
{
	out_file_name = $"{Path.GetFileNameWithoutExtension(in_file_name)}输出{Path.GetExtension(in_file_name)}";
}

using FileStream in_txt = File.Open(in_file_name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
using StreamReader reader = new(in_txt);

using FileStream out_txt = File.Open(out_file_name, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
out_txt.SetLength(0);
using StreamWriter writer = new(out_txt);

while (true)
{
	string? line = await reader.ReadLineAsync();
	if (line == null)
	{
		// 读取到文件尾了，退出循环
		break;
	}

	int index = line.IndexOf(" ");
	if (index == -1)
	{
		// 没有找到空格，说明此行只含有一个行号，写入空行
		Console.WriteLine();
		await writer.WriteLineAsync();
		continue;
	}

	// 找到空格了，从空格开始截取后面的内容
	line = line[index..];
	Console.WriteLine(line);
	await writer.WriteLineAsync(line);
}

await writer.FlushAsync();
await out_txt.FlushAsync();
Console.WriteLine("处理完成");