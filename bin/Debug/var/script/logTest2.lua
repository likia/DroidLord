init(0)
luaExitIfCall(true);

local sdPath = getSDCardPath();
if sdPath == nil then
    --dialog("该设备没有sd卡");
	rzlj = "/data/local/tmp/Overseer.log"
	tplj = "/data/local/tmp/img/"
else
    rzlj = getSDCardPath().."/data/local/tmp/Overseer.log"
	tplj = getSDCardPath().."/data/local/tmp/img/"
end

snapshot("/data/local/tmp/img/"..os.time()..".png", 0, 0, 1279, 719); 
toast(rzlj);
file = io.open(rzlj,"a");

lvl = {'MESSAGE', 'WARNING', 'ERROR'};
file:write("完成|" .. os.time() ..  "|" .. lvl[os.time() % 4] .. "|无截图|\n")
file:close();