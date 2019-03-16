init(0)
luaExitIfCall(true);

	local F,err=io.open("/sdcard/data/local/tmp/img","r+");
	t = type(string.find(err ,"Is a directory" ));
	
	if t == "number" then 
		rzlj = "/sdcard/data/local/tmp/Overseer.log"
		tplj = "/sdcard/data/local/tmp/img/"
		
	else
		
		local F,err=io.open("/data/local/tmp/img","r+");
		t = type(string.find(err ,"Is a directory" ));
		
		if t == "number" then 
			rzlj = "/data/local/tmp/Overseer.log"
			tplj = "/data/local/tmp/img/"
		else
			dialog("无日志目录")
		end		
	end

snapshot("/data/local/tmp/img/"..os.time()..".png", 0, 0, 1279, 719); 

file = io.open(rzlj,"a");
lvl = {'MESSAGE', 'WARNING', 'ERROR'};
file:write("完成|" .. os.time() ..  "|" .. lvl[os.time() % 4] .. "|无截图|\n")
file:close();