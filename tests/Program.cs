using Yaz0Library;

string c1Path = "D:\\Bin\\Yaz0Library\\Enemy_Lynel_Dark.bactorpack";
string c2Path = "D:\\Bin\\Yaz0Library\\Enemy_Bokoblin_Dark.bactorpack";

var c1 = Yaz0.Compress(File.ReadAllBytes(c1Path));
var c2 = Yaz0.Compress(File.ReadAllBytes(c2Path));

using (FileStream writer = File.Create("D:\\c1.sbactorpack")) {
    writer.Write(c1);
}

using (FileStream writer = File.Create("D:\\c2.sbactorpack")) {
    writer.Write(c2);
}