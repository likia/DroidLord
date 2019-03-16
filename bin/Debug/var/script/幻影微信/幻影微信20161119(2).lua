init(0)
luaExitIfCall(true);


flag = deviceIsLock(); --判断屏幕是否锁定
if flag == 0 then
	mSleep(800);
   
else
    unlockDevice();    --解锁屏幕
	mSleep(800);
	unlockDevice();    --解锁屏幕
end

::getInfo:: --设置跳转标签
local sz = require("sz")
local json = sz.json
local w,h = getScreenSize(); --w 720宽度  h 1280高度
w0 = w / 720
h0 = h / 1280
jb = 0
MyTable = {
    ["style"] = "default",
    ["width"] = w,
    ["height"] = h,
    ["config"] = "hyconfig.dat",
    ["timer"] = 3,
    views = {
        {
            ["type"] = "ComboBox",                       --下拉框，hy1
            ["list"] = "0.无,1.文字朋友圈,2.图文朋友圈,3.文字群发,4.通讯录好友,5.附近的人,6.微信群信息,7.加微信群好友",--7个下拉选项，序号从0开始，即选项1编号为0，选项2编号为1，依此类推
            ["select"] = "0",                       --默认选择选项2
        },{
            ["type"] = "Edit",        --输入框，hy2
            ["prompt"] = "发送的文本内容",--编辑框中无任何内容时显示的底色文本
            ["text"] = "内容1",        --界面载入时已经存在于编辑框中的文本
        },{
            ["type"] = "ComboBox",                       --下拉框，hy3
            ["list"] = "1,2,3,4,5,6,7,8,9",--7个下拉选项，序号从0开始，即选项1编号为0，选项2编号为1，依此类推
            ["select"] = "0",                       --默认选择选项2
        },{
            ["type"] = "Edit",        --输入框，hy4
            ["prompt"] = "微信群标题关键字",--编辑框中无任何内容时显示的底色文本
            ["text"] = "彩虹",        --界面载入时已经存在于编辑框中的文本
        },--[[{
            ["type"] = "Label",
            ["text"] = "设置",
            ["size"] = 25,
            ["align"] = "center",
            ["color"] = "0,0,255",
        },
        {
            ["type"] = "RadioGroup",                     --单选框，input1
            ["list"] = "选项1,选项2,选项3,选项4,选项5,选项6,选项7",--7个单选项，序号从0开始，即选项1编号为0，选项2编号为1，依此类推
            ["select"] = "1",                    --默认选择选项2
        },
        {
            ["type"] = "Edit",        --输入框，input2
            ["prompt"] = "请输入一个数字",--编辑框中无任何内容时显示的底色文本
            ["text"] = "默认值",        --界面载入时已经存在于编辑框中的文本
        },
        {
            ["type"] = "CheckBoxGroup",                  --多选框，input3
            ["list"] = "选项1,选项2,选项3,选项4,选项5,选项6,选项7",--7个多选项
            ["select"] = "3@5",                        --默认选择选项3和选项5
        },
        {
            ["type"] = "ComboBox",                       --下拉框，input4
            ["list"] = "选项1,选项2,选项3,选项4,选项5,选项6,选项7",--7个下拉选项，序号从0开始，即选项1编号为0，选项2编号为1，依此类推
            ["select"] = "1",                       --默认选择选项2
        },]]
    }
}
local MyJsonString = json.encode(MyTable);
ret, hy1,hy2,hy3,hy4 = showUI(MyJsonString);--返回值ret,hy1 脚本类型 0=无 1=文字朋友圈   hy2 文字内容  hy3 群发图片数量

	if ret == 0 then
		goto guanbi
	end


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
			goto jieshu
		end
		
	end

--[[
local sdPath = getSDCardPath();
if sdPath == nil then
    --dialog("该设备没有sd卡");
	rzlj = "/data/local/tmp/Overseer.log"
	tplj = "/data/local/tmp/img/"
else
    rzlj = getSDCardPath().."/data/local/tmp/Overseer.log"
	tplj = getSDCardPath().."/data/local/tmp/img/"
end
	]]


	if tonumber(hy1) == 0 then
		goto guanbi
	else
		
		if tonumber(hy1) == 1 then
			hyjb = "文字朋友圈"
		end
		
		if tonumber(hy1) == 2 then
			hyjb = "图文朋友圈"
		end
		
		if tonumber(hy1) == 3 then
			hyjb = "文字群发"
		end
		
		if tonumber(hy1) == 4 then
			hyjb = "批量添加通讯录好友"
		end
		
		if tonumber(hy1) == 5 then
			hyjb = "批量附近的人"
		end
		
		if tonumber(hy1) == 6 then
			hyjb = "微信群信息"
		end
		
		if tonumber(hy1) == 7 then
			hyjb = "批量加微信群好友"
		end
		
		
		
		toast(hyjb,5);
	end
	

	
	--------------------------启动微信APP
	closeApp("com.tencent.mm");  --关闭微信
	mSleep(2000);
	r = runApp("com.tencent.mm")--,"com.tencent.mm.plugin.nearby.ui.NearbyFriendsIntroUI") --启动微信并打开朋友圈
	toast("APP加载中");
	mSleep(10*1000);
 
	if r == 0 then
		
		
	else
		jjb = "WARNING" 
		jjs = "APP启动失败"
		xxq = "未安装 微信APP 或 微信APP 异常需要重新安装"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
		
	end
	
	
	
	
	
	
	
	
	
dl0 = 0
repeat
	
	x, y = findColorInRegionFuzzy(0x46c01b, 98, 21, 1100, 683, 1277);  --检测是否在登录状态下
	if x ~= -1 and y ~= -1 then        --如果在指定区域找到某点符合条件
		toast("APP启动成功"); 
		dl0 = dl0 + 100
	else                               --如果找不到符合条件的点
		dl0 = dl0 + 1
		mSleep(10 * 1000);
	end
	
until(dl0>5)


if dl0 < 90 then
	
		--dialog("未登录微信帐号",3);
		jjb = "WARNING" 
		jjs = "未登录微信帐号"
		xxq = "未登录微信帐号"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu

end

	
	
	-----------------------筛选执行脚本类型
	
	if tonumber(hy1) == 1 then
		goto wenzipengyouquan
	end
	
	if tonumber(hy1) == 2 then
		goto tuwenpengyouquan
	end
	
	if tonumber(hy1) == 3 then
		goto wenziqunfa
	end
	
	if tonumber(hy1) == 4 then
		goto tongxunlu
	end
	
	if tonumber(hy1) == 5 then
		goto fujinderen
	end
	
	if tonumber(hy1) == 6 then
		goto weixinqunxinxi
	end
	
	if tonumber(hy1) == 7 then
		goto weixinqunxinxi
	end
-----------------------------1.文字朋友圈
::wenzipengyouquan::

toast("文字朋友圈"); 
mSleep(100);
setScreenScale(true, 720, 1280)  --分辨路设置成720.1280
mSleep(100);
os.execute("am start com.tencent.mm/.plugin.sns.ui.SnsLongMsgUI");  --文字朋友圈
--[[
r = runApp("com.tencent.mm","com.tencent.mm.plugin.sns.ui.SnsLongMsgUI") --启动微信并打开添加好友
toast("微信加载中");
mSleep(10 * 1000);

	if r == 0 then
		toast("微信启动成功"); 
		
	else
		dialog("启动失败 错误01",3);
		goto jieshu
	end]]
mSleep(10*800);
----------------
	x, y = findColorInRegionFuzzy(0x1aad19, 90, 200, 1080, 530, 1230); --找发送  坐标
	if x ~= -1 and y ~= -1 then  
		touchDown(1, x, y);      --那么单击发送
		mSleep(100);
		touchUp(1, x, y);
		toast("初始化文字朋友圈"); 
		
		
	else
		
		--dialog("异常退出 错误02",3);
		jjb = "ERROR" 
		jjs = "异常退出"
		xxq = "(200, 1080)到 (530, 1230) 未识别到 0x1aad19 我知道了 按钮"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	end
-------------


ml0 = 0
repeat
	x, y = findColorInRegionFuzzy(0x2f6732, 90, 560, 55, 717, 145); --找发送  坐标
	if x ~= -1 and y ~= -1 then
		a = y + 100
		touchDown(1, x, a);      
		mSleep(100);
		touchUp(1, x, a);
		ml0 = ml0 + 100
	else
		ml0 = ml0 + 1
		mSleep(1000);
	end
until(ml0>9)



if ml0 < 90 then
		--dialog("异常退出 错误05",3);
		jjb = "ERROR" 
		jjs = "异常退出"
		xxq = "(560, 55)到 (717, 145) 未识别到 0x2f6732 暗绿色 发送 按钮,所以无法点击文字编辑区域"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu

end

mSleep(500);                --延迟 0.5 秒
inputText(hy2);          --写出字符串
mSleep(1500);

---------------

	x, y = findColorInRegionFuzzy(0x2f6732, 90, w-200, 50, w-1, h/9); --找发送  坐标
	if x ~= -1 and y ~= -1 then  
		--dialog("发送内容为空 错误03",3);
		jjb = "WARNING" 
		jjs = "发送内容为空"
		xxq = "(560, 55)到 (717, 145) 识别到 0x2f6732 暗绿色 发送 按钮,无法点击 发送内容为空也许为空"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
		
	else
		x, y = findColorInRegionFuzzy(0x1aad19, 90, w-200, 50, w-1, h/9); --找发送  坐标
		if x ~= -1 and y ~= -1 then  
			touchDown(1, x, y);      --那么单击发送
			mSleep(100);
			touchUp(1, x, y);
			toast("发送成功"); 
			mSleep(2000);
		else
			--dialog("异常退出 错误04",3);
			jjb = "ERROR" 
			jjs = "异常 无法点击发送按钮"
			xxq = "(560, 55)到 (717, 145) 未识别到 0x1aad19 绿色 发送 按钮,无法点击 发送"
			toast(jjs,3);
			snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
			file = io.open(rzlj,"a");
			file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
			file:close();
			goto jieshu
		end
	end

	file = io.open(rzlj,"a");
	file:write("完成|"..hyjb.."|MESSAGE|无截图|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
	file:close();

goto jieshu
------------------------2.图文朋友圈
::tuwenpengyouquan::


	toast("图文朋友圈"); 
	mSleep(100);
	setScreenScale(true, 720, 1280)  --分辨路设置成720.1280
	mSleep(100);
	os.execute("am start com.tencent.mm/.plugin.sns.ui.SnsTimeLineUI");  --图文朋友圈
	--[[
	r = runApp("com.tencent.mm","com.tencent.mm.plugin.sns.ui.SnsTimeLineUI") --启动微信并打开添加好友
	toast("微信加载中");
	mSleep(10 * 1000);
	if r == 0 then
		toast("微信启动成功"); 
		
	else
		dialog("启动失败 错误01",3);
	end]]
	mSleep(10*800)
-------------------

c = 10
repeat
	
	x, y = findColorInRegionFuzzy(0xffffff, 95, 622, 66, 700, 130); --找发送  坐标
	if x ~= -1 and y ~= -1 then  
		touchDown(1, x, y);      --那么单击发送
		mSleep(100);
		touchUp(1, x, y);
		mSleep(100);
		toast("初始化图文朋友圈"); 
		mSleep(2000);
		
	else
		--dialog("异常退出 错误02",3);
		jjb = "ERROR" 
		jjs = "异常 无 相机 按钮"
		xxq = "(622, 66)到 (700, 130) 未识别到 0xffffff 白色 相机 按钮"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	end
	
	
	
	mSleep(3000);

	x, y = findColorInRegionFuzzy(0x1aad19, 90, 415, 803, 639, 921); --找 我知道
	if x ~= -1 and y ~= -1 then  
		touchDown(1, x, y);      --那么单击发送
		mSleep(100);
		touchUp(1, x, y);
		mSleep(100);
		toast("我知道了"); 
		mSleep(2000);
		
	else
		
	end
	




	x, y = findColorInRegionFuzzy(0x6a6a6a, 90, 111, 595, 187, 640); --找发送  坐标
	if x ~= -1 and y ~= -1 then  
		touchDown(1, x, y);      --那么单击发送
		mSleep(100);
		touchUp(1, x, y);
		mSleep(3000);
	else
		
		--dialog("异常退出 错误03",3);
		jjb = "ERROR" 
		jjs = "异常 无法点击 照片"
		xxq = '(111, 595)到 (187, 640) 未识别到 0x6a6a6a "片" 字'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	end
	
	x, y = findColorInRegionFuzzy(0x1aad19, 90, 415, 803, 639, 921); --找 我知道
	if x ~= -1 and y ~= -1 then  
		touchDown(1, x, y);      --那么单击发送
		mSleep(100);
		touchUp(1, x, y);
		mSleep(100);
		toast("我知道了"); 
		mSleep(2000);
		
	else
		
	end
	
	mSleep(2000);
	x, y = findColorInRegionFuzzy(0xffffff, 95, 622, 66, 700, 130); --找发送  坐标
	if x ~= -1 and y ~= -1 then  
		
		toast("图文朋友圈"); 
		mSleep(2000);
		
	else
		c = c + 2
		mSleep(2000);
	end
until(c>11)

-----------------------------------------
mSleep(3000);
w0 = w / 3  -- w=720 w0=240 48
h0 = w0 * 3 / 4
	tp1 = 10
	tp2 = 10
	tp3 = 10
	tp4 = 10
	tp5 = 10
	tp6 = 10
	tp7 = 10
	tp8 = 10
	tp9 = 10
	
	repeat
		x, y = findColorInRegionFuzzy(0x1aad19, 95, w0*2-w0/5 , h0 , w0*2-w0/5+30 , h0+30 ); --找发送  坐标
		if x ~= -1 and y ~= -1 then  
			tp1 = tp1 + 100
			
		else
			
			touchDown(1, w0*2-w0/5, h0+30);      --那么单击发送
			mSleep(100);
			touchUp(1, w0*2-w0/5, h0+30);
			mSleep(100);
			toast("图片 1"); 
			tp1 = tp1 + 1
			mSleep(2000);
			
		end
	until(tp1>14)

	if tonumber(tp1) < 99 then
		--dialog("无法选择图片 1",3);
		jjb = "ERROR" 
		jjs = "无法选择图片 1"
		xxq = "无法选择图片 1"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	end


	if tonumber(hy3) >= 1 then
		repeat
			x, y = findColorInRegionFuzzy(0x1aad19, 99, w0*3-w0/5 , h0 , w0*3-w0/5+30 , h0+30); --图片2  坐标
			if x ~= -1 and y ~= -1 then  
				tp2 = tp2 + 100
			else
				touchDown(1, w0*3-w0/5, h0+30);      --那么单击发送
				mSleep(100);
				touchUp(1, w0*3-w0/5, h0+30);
				mSleep(100);
				toast("图片 2"); 
				tp2 = tp2 + 1
				mSleep(2000);
			end
		until(tp2>12)
		
		if tonumber(tp2) < 100 then
			--dialog("无法选择图片 2",3);
			jjb = "ERROR" 
			jjs = "无法选择图片 2"
			xxq = "无法选择图片 2"
			toast(jjs,3);
			snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
			file = io.open(rzlj,"a");
			file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
			file:close();
			goto jieshu
		end
	end



	if tonumber(hy3) >= 2 then
		repeat
			x, y = findColorInRegionFuzzy(0x1aad19, 99,  w0*1-w0/5 , h0+w0 , w0*1-w0/5+30 , h0+30+w0); --图片3  坐标
			if x ~= -1 and y ~= -1 then  
				tp3 = tp3 + 100
			else
				touchDown(1, w0*1-w0/5, h0+30+w0);      --那么单击发送
				mSleep(100);
				touchUp(1, w0*1-w0/5, h0+30+w0);
				mSleep(100);
				toast("图片 3"); 
				tp3 = tp3 + 1
				mSleep(2000);
			end
		until(tp3>12)
		
		if tonumber(tp3) < 100 then
			--dialog("无法选择图片 3",3);
			jjb = "ERROR" 
			jjs = "无法选择图片 3"
			xxq = "无法选择图片 3"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
			goto jieshu
		end
	end
	
	
	if tonumber(hy3) >= 3 then
		repeat
			x, y = findColorInRegionFuzzy(0x1aad19, 99,  w0*2-w0/5 , h0+w0 , w0*2-w0/5+30 , h0+30+w0); --图片4  坐标
			if x ~= -1 and y ~= -1 then  
				tp4 = tp4 + 100
			else
				touchDown(1, w0*2-w0/5, h0+30+w0);      --那么单击发送
				mSleep(100);
				touchUp(1, w0*2-w0/5, h0+30+w0);
				mSleep(100);
				toast("图片 4"); 
				tp4 = tp4 + 1
				mSleep(2000);
			end
		until(tp4>12)
		
		if tonumber(tp4) < 100 then
			--dialog("无法选择图片 4",3);
			jjb = "ERROR" 
			jjs = "无法选择图片 4"
			xxq = "无法选择图片 4"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
			goto jieshu
		end
	end




	if tonumber(hy3) >= 4 then
		
		repeat
			x, y = findColorInRegionFuzzy(0x1aad19, 99, w0*3-w0/5 , h0+w0 , w0*3-w0/5+30 , h0+30+w0); --图片5  坐标
			if x ~= -1 and y ~= -1 then  
				tp5 = tp5 + 100
			else
				touchDown(1, w0*3-w0/5, h0+30+w0);      --那么单击发送
				mSleep(100);
				touchUp(1, w0*3-w0/5, h0+30+w0);
				mSleep(100);
				toast("图片 5"); 
				tp5 = tp5 + 1
				mSleep(2000);
			end
		until(tp5>12)
		
		
		if tonumber(tp5) < 100 then
			--dialog("无法选择图片 5",3);
			jjb = "ERROR" 
			jjs = "无法选择图片 5"
			xxq = "无法选择图片 5"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
			goto jieshu
		end
	end
	




	if tonumber(hy3) >= 5 then
		repeat
			x, y = findColorInRegionFuzzy(0x1aad19, 99, w0*1-w0/5 , h0+w0*2 , w0*1-w0/5+30 , h0+30+w0*2); --图片6  坐标
			if x ~= -1 and y ~= -1 then  
				tp6 = tp6 + 100
			else
				touchDown(1, w0*1-w0/5, h0+30+w0*2);      --那么单击发送
				mSleep(100);
				touchUp(1, w0*1-w0/5, h0+30+w0*2);
				mSleep(100);
				toast("图片 6"); 
				tp6 = tp6 + 1
				mSleep(2000);
			end
		until(tp6>12)
		
		if tonumber(tp6) < 100 then
			--dialog("无法选择图片 6",3);
			jjb = "ERROR" 
			jjs = "无法选择图片 6"
			xxq = "无法选择图片 6"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
			goto jieshu
		end
	end





	if tonumber(hy3) >= 6 then
		repeat
			x, y = findColorInRegionFuzzy(0x1aad19, 99,w0*2-w0/5 , h0+w0*2 , w0*2-w0/5+30 , h0+30+w0*2); --图片7  坐标
			if x ~= -1 and y ~= -1 then  
				tp7 = tp7 + 100
			else
				touchDown(1, w0*2-w0/5, h0+30+w0*2);      --那么单击发送
				mSleep(100);
				touchUp(1, w0*2-w0/5, h0+30+w0*2);
				mSleep(100);
				toast("图片 7"); 
				tp7 = tp7 + 1
				mSleep(2000);
			end
		until(tp7>12)
		
		if tonumber(tp7) < 100 then
			--dialog("无法选择图片 7",3);
			jjb = "ERROR" 
			jjs = "无法选择图片 7"
			xxq = "无法选择图片 7"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
			goto jieshu
		end
	end
	
	
	
	if tonumber(hy3) >= 7 then
		repeat
			x, y = findColorInRegionFuzzy(0x1aad19, 99, w0*3-w0/5 , h0+w0*2 , w0*3-w0/5+30 , h0+30+w0*2); --图片8  坐标
			if x ~= -1 and y ~= -1 then  
				tp8 = tp8 + 100
			else
				touchDown(1, w0*3-w0/5, h0+30+w0*2);      --那么单击发送
				mSleep(100);
				touchUp(1, w0*3-w0/5, h0+30+w0*2);
				mSleep(100);
				toast("图片 8"); 
				tp8 = tp8 + 1
				mSleep(2000);
				
			end
		until(tp8>12)
		
		if tonumber(tp8) < 100 then
			--dialog("无法选择图片 8",3);
			jjb = "ERROR" 
			jjs = "无法选择图片 8"
			xxq = "无法选择图片 8"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
			goto jieshu
		end
	end
	
	
	if tonumber(hy3) >= 8 then
		repeat
			x, y = findColorInRegionFuzzy(0x1aad19, 99,w0*1-w0/5 , h0+w0*3 , w0*1-w0/5+30 , h0+30+w0*3); --图片9  坐标
			if x ~= -1 and y ~= -1 then  
				tp9 = tp9 + 100
			else
				touchDown(1, w0*1-w0/5, h0+30+w0*3);      --那么单击发送
				mSleep(100);
				touchUp(1, w0*1-w0/5, h0+30+w0*3);
				mSleep(100);
				toast("图片 9");
				tp9 = tp9 + 1
				mSleep(2000);
			end
		until(tp9>12)
		
		if tonumber(tp9) < 100 then
			--dialog("无法选择图片 9",3);
			jjb = "ERROR" 
			jjs = "无法选择图片 9"
			xxq = "无法选择图片 9"
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
			goto jieshu
		end
		
	end













	x, y = findColorInRegionFuzzy(0x1aad19, 80, w-200, 50, w-1, h/9); --找完成  坐标
if x ~= -1 and y ~= -1 then  
	touchDown(1, x, y);      --那么单击发送
	mSleep(100);
    touchUp(1, x, y);
	mSleep(4000);
else
	
    --dialog("异常退出 错误05",3);
	jjb = "ERROR" 
	jjs = "异常退出"
	xxq = '(600, 60)到 (707, 130) 未识别到 0x1aad19 绿色 "完成" 按钮'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
	goto jieshu
end
--------------------------

	x, y = findColorInRegionFuzzy(0xbbbbbb, 90, 80, 170, 220, 200); --找完成  坐标
if x ~= -1 and y ~= -1 then  
	touchDown(1, x, y);      --那么单击发送
	mSleep(100);
    touchUp(1, x, y);
	mSleep(1000);
else
	
    --dialog("异常退出 错误06",3);
	jjb = "ERROR" 
	jjs = "异常退出"
	xxq = '(80, 170)到 (220, 200) 未识别到 0xbbbbbb 灰色 "这一刻的想法..." 文字'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
	goto jieshu
end




mSleep(500);                --延迟 0.5 秒
inputText(hy2);          --写出字符串
mSleep(1500);

	x, y = findColorInRegionFuzzy(0x2f6732, 90, w-200, 50, w-1, h/9); --找灰绿色发送  坐标
	if x ~= -1 and y ~= -1 then  
		--dialog("发送内容为空 错误03",3);
		
		jjb = "WARNING" 
		jjs = "发送内容为空"
		xxq = '(560, 55)到 (717, 145) 识别到 0x2f6732 灰绿色 "发送" 按钮'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
		
	else
		x, y = findColorInRegionFuzzy(0x1aad19, 80, w-200, 50, w-1, h/9); --找发送  坐标
		if x ~= -1 and y ~= -1 then  
			touchDown(1, x, y);      --那么单击发送
			mSleep(100);
			touchUp(1, x, y);
			toast("发送成功"); 
			mSleep(2000);
		else
			--dialog("异常退出 错误04",3);
			jjb = "ERROR" 
			jjs = "异常退出"
			xxq = '(560, 55)到 (717, 145) 未识别到 0x1aad19 绿色 "发送" 按钮'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
			goto jieshu
		end
	end


	file = io.open(rzlj,"a");
	file:write("完成|"..hyjb.."|MESSAGE|无截图|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
	file:close();


goto jieshu
---------------------------3.文字群发
::wenziqunfa::

os.execute("am start com.tencent.mm/.plugin.masssend.ui.MassSendSelectContactUI");  --文字群发
--[[
r = runApp("com.tencent.mm","com.tencent.mm.plugin.masssend.ui.MassSendSelectContactUI") --启动微信并打开添加好友
toast("微信加载中");
mSleep(10 * 1000);

	if r == 0 then
		toast("微信启动成功"); 
		
	else
		dialog("启动失败 错误01",3);
	end]]
mSleep(10*800);
-----------------
e = 10
	repeat  --
		x, y = findColorInRegionFuzzy(0x2f6732, 90, 590, 57, 717, 145); --找下一步  坐标
		if x ~= -1 and y ~= -1 then  
			a = y + 125
			touchDown(1, x, a);      --那么单击单击全选
			mSleep(100);
			touchUp(1, x, a);
			toast("全选"); 
			mSleep(2000);
			
			b, c = findColorInRegionFuzzy(0x1aad19, 90, 590, 57, 717, 145); --找下一步  坐标
			if b ~= -1 and c ~= -1 then  
				touchDown(1, b, c);      --那么单击下一步
				mSleep(100);
				touchUp(1, b, c);
				e = e + 2
			else
				--dialog("异常退出 错误02",3);
				jjb = "ERROR" 
				jjs = "异常退出"
				xxq = '(590, 57)到 (717, 145) 未识别到 0x1aad19 绿色 "下一步" 按钮'
				toast(jjs,3);
				snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
				file = io.open(rzlj,"a");
				file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
				file:close();
				goto jieshu
			end
			
		else
			b, c = findColorInRegionFuzzy(0x1aad19, 90, 590, 57, 717, 145); --找下一步  坐标
			if b ~= -1 and c ~= -1 then  
				d = c + 125
				touchDown(1, b, d);      --那么单击全选
				mSleep(100);
				touchUp(1, b, d);
			else
				--dialog("异常退出 错误03",3);
				jjb = "ERROR" 
				jjs = "异常退出"
				xxq = '(590, 57)到 (717, 145) 未识别到 0x1aad19 绿色 "下一步" 按钮'
				toast(jjs,3);
				snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
				file = io.open(rzlj,"a");
				file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
				file:close();
				goto jieshu
			end
		end
		mSleep(1000);
	until(e>11)
-----------------------------------------------------


mSleep(500);                --延迟 0.5 秒
inputText(hy2);          --写出字符串
mSleep(1500);
------------------------------------------------------
	x, y = findColorInRegionFuzzy(0x1aad19, 90, 560, 580, 714, 1276); --找发送  坐标
	if x ~= -1 and y ~= -1 then  
		touchDown(1, x, y);      --那么单击发送
		mSleep(100);
		touchUp(1, x, y);
		toast("发送"); 
		mSleep(2000);
		
	else
		--dialog("异常退出 错误05",3);
		jjb = "ERROR" 
		jjs = "群发内容为空"
		xxq = '(560, 580)到 (714, 1276) 未识别到 0x1aad19 绿色 "发送" 按钮'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	end
-----------------------------------
	x, y = findColorInRegionFuzzy(0x1aad19, 90, 230, 1150, 505, 1280); --找发送  坐标
	if x ~= -1 and y ~= -1 then  
		
		toast("发送成功"); 
	else
		--dialog("异常退出 错误06",3);
		jjb = "ERROR" 
		jjs = "群发内容为空"
		xxq = '(560, 580)到 (714, 1276) 未识别到 0x1aad19 绿色 "发送" 按钮'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	end
	
	
	file = io.open(rzlj,"a");
	file:write("完成|"..hyjb.."|MESSAGE|无截图|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
	file:close();
	
	
goto jieshu
-------------------------------4.批量添加通讯录好友
::tongxunlu::

toast("添加通讯录脚本开始"); 
mSleep(100);
setScreenScale(true, 720, 1280)  --分辨路设置成7200.12800
mSleep(100);

os.execute("am start com.tencent.mm/.plugin.subapp.ui.pluginapp.AddMoreFriendsByOtherWayUI");  --批量添加通讯录好友
--[[
r = runApp("com.tencent.mm","com.tencent.mm.plugin.subapp.ui.pluginapp.AddMoreFriendsByOtherWayUI") --批量添加通讯录好友
toast("微信加载中");
mSleep(10 * 1000);
 
	if r == 0 then
		toast("微信启动成功"); 
		
	else
		dialog("启动失败",3);
	end]]
mSleep(10*800);
---------------------------------------------

	x, y = findColorInRegionFuzzy(0x4ec326, 80, 15, 166, 109, 407); --找确定  坐标
	if x ~= -1 and y ~= -1 then  --找添加手机联系人
		
		touchDown(1, x, y);      --那么单击添加手机联系人
		mSleep(100);
		touchUp(1, x, y);
		toast("添加手机联系人"); 
	else
		--dialog("异常退出",3);
		jjb = "ERROR" 
		jjs = '未"添加手机联系人"'
		xxq = '(15, 166)到 (109, 407) 未识别到 0x4ec326 绿色 "电话" 按钮'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	end

	mSleep(1000);
	
--------------------------
	for i = 1, 2 do      
		x, y = findColorInRegionFuzzy(0x1aad19, 80, 216, 833, 334, 934); --找上传通讯录  坐标
		if x ~= -1 and y ~= -1 then  --找添加手机联系人
			
			touchDown(1, x, y);      --那么单击添加手机联系人
			mSleep(100);
			touchUp(1, x, y);
			toast("初始化添加手机联系人"); 
			mSleep(500);
			
			a, b = findColorInRegionFuzzy(0x7f7f7f, 80, 402, 789, 431, 814); --找灰否  坐标
			if a ~= -1 and b ~= -1 then  
				c = a + 544 - 414
				touchDown(1, c, b);      --那么单击添加手机联系人
				mSleep(100);
				touchUp(1, c, b);
			else  --否则
				touchDown(1, x, y);      --那么单击添加手机联系人
				mSleep(100);
				touchUp(1, x, y);
				a, b = findColorInRegionFuzzy(0x7f7f7f, 80, 402, 789, 431, 814); --找灰否  坐标
				if a ~= -1 and b ~= -1 then  
					c = a + 544 - 414
					touchDown(1, c, b);      --那么单击添加手机联系人
					mSleep(100);
					touchUp(1, c, b);
				else
					--dialog("异常退出",3);
					jjb = "ERROR" 
					jjs = '异常退出'
					xxq = '未知'
					toast(jjs,3);
					snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
					file = io.open(rzlj,"a");
					file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
					file:close();
					goto jieshu
				end
				
			end
			
			
		else
			mSleep(100);
		end
		mSleep(1000);
	end  --循环结束
mSleep(2000);
-------------------点击添加脚本

------------------循环翻页脚本

z = 10
	repeat  --循环翻页
		x, y = findImageInRegionFuzzy("pd.png", 90, 149, 965, 292, 1177, 0xffffff);  -- 判断是否翻到底 坐标
		if x ~= -1 and y ~= -1 then        --如果在指定区域找到某图片符合条件
		  z = z + 2 --如果有pd.png图片 z+2 循环停止
			toast("添加联系人完成"); 
			mSleep(1038)
		else                               --如果找不到符合条件的图片
			---------------------------------------
			v = 10
			repeat
				x, y = findColorInRegionFuzzy(0x1aad19, 80, 526, 130, 718, 1279); --找添加  坐标
				if x ~= -1 and y ~= -1 then  --找添加手机联系人
					
					touchDown(1, x, y);      --那么单击添加手机联系人
					mSleep(100);
					touchUp(1, x, y);
					mSleep(1800);
				else
					v = v + 2
				end
			until(v>11)
			---------------------------------------
			
			
		   snapshot("pd.png", 169, 985, 272, 1157); --截图  坐标
			--下拉
			mSleep(870)
			touchDown(1,129,758)
			mSleep(12)
			touchMove(1,129,757)
			mSleep(16)
			touchMove(1,136,746)
			mSleep(16)
			touchMove(1,137,731)
			mSleep(10)
			touchMove(1,137,720)
			mSleep(12)
			touchMove(1,137,705)
			mSleep(13)
			touchMove(1,139,689)
			mSleep(12)
			touchMove(1,139,673)
			mSleep(13)
			touchMove(1,139,656)
			mSleep(13)
			touchMove(1,138,641)
			mSleep(13)
			touchMove(1,137,625)
			mSleep(13)
			touchMove(1,137,610)
			mSleep(15)
			touchMove(1,137,594)
			mSleep(11)
			touchMove(1,137,576)
			mSleep(12)
			touchMove(1,136,557)
			mSleep(14)
			touchMove(1,136,537)
			mSleep(12)
			touchMove(1,136,518)
			mSleep(15)
			touchMove(1,137,501)
			mSleep(11)
			touchMove(1,137,484)
			mSleep(15)
			touchMove(1,138,466)
			mSleep(11)
			touchMove(1,139,446)
			mSleep(14)
			touchMove(1,139,425)
			mSleep(14)
			touchMove(1,140,405)
			mSleep(10)
			touchMove(1,141,387)
			mSleep(15)
			touchMove(1,143,372)
			mSleep(10)
			touchMove(1,144,361)
			mSleep(13)
			touchMove(1,145,351)
			mSleep(13)
			touchMove(1,145,340)
			mSleep(13)
			touchMove(1,145,328)
			mSleep(13)
			touchMove(1,145,314)
			mSleep(13)
			touchMove(1,146,302)
			mSleep(14)
			touchMove(1,146,292)
			mSleep(11)
			touchMove(1,147,283)
			mSleep(17)
			touchMove(1,147,275)
			mSleep(9)
			touchMove(1,148,267)
			mSleep(15)
			touchMove(1,149,260)
			mSleep(11)
			touchMove(1,151,255)
			mSleep(15)
			touchMove(1,152,251)
			mSleep(12)
			touchMove(1,152,247)
			mSleep(11)
			touchMove(1,152,244)
			mSleep(13)
			touchMove(1,153,241)
			mSleep(13)
			touchMove(1,153,239)
			mSleep(14)
			touchMove(1,153,238)
			mSleep(12)
			touchMove(1,154,238)
			mSleep(1038)
		end
		
	until(z>11)
	file = io.open(rzlj,"a");
	file:write("完成|"..hyjb.."|MESSAGE|无截图|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
	file:close();




goto jieshu
-------------------------5.附近的人
::fujinderen::



toast("添加身边的人脚本开始"); 
setScreenScale(true, 720, 1280)
mSleep(1000);
os.execute("am start com.tencent.mm/.plugin.nearby.ui.NearbyFriendsIntroUI");  --批量附近的人
--[[
r = runApp("com.tencent.mm","com.tencent.mm.plugin.nearby.ui.NearbyFriendsIntroUI") --附近的人
toast("APP加载中");
mSleep(10 * 1000);
 
	if r == 0 then
		toast("APP启动成功"); 
		
	else
		dialog("启动失败",3);
	end]]

mSleep(8*1000);

 
x, y = findColorInRegionFuzzy(0x1aad19, 80, 157, 537, 240, 950); --初始化附近的人
	if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
		
		touchDown(1, x, y);      --那么单击该点
		mSleep(100);
		touchUp(1, x, y);
		mSleep(1000);
		--------------------------
		x, y = findColorInRegionFuzzy(0x1aad19, 50, 110, 437, 601, 912); --找确定
		if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
			
			a = x - 377
			b = y - 81
			touchDown(2, a, b);      --那么单击不在提醒
			mSleep(100);
			touchUp(2, a, b);
			
			mSleep(1000);
			touchDown(1, x, y);      --那么单击确定
			mSleep(100);
			touchUp(1, x, y);
			mSleep(2000);
			
		else                         --如果找不到符合条件的点
			--dialog("找不到确认  失败",3);
			jjb = "ERROR" 
			jjs = '未找到"确认"按钮'
			xxq = '(110, 437)到 (601, 912) 未识别到 0x1aad19 绿色 "确定" 按钮'
			toast(jjs,3);
			snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
			file = io.open(rzlj,"a");
			file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
			file:close();
			goto jieshu
		end
		-----------------------------
	else                         --如果找不到符合条件的点
		toast("初始化完成",0);
	end
mSleep(2000);


abc = 0
repeat
	x, y = findColorInRegionFuzzy(0x353535, 90, 512, 621, 532, 636); ---正在确定你的位置
	if x ~= -1 and y ~= -1 then  
		mSleep(1000);
		abc = abc + 1
		toast("等待位置信息 "..abc,0);
	else                         --如果找不到符合条件的点
		abc = abc + 900
	end
until(abc>60)

	if abc <= 800 then  
		--dialog("无法定位位置信息",5);
		jjb = "WARNING" 
		jjs = '获取定位信息超时'
		xxq = '未能在60秒内获取到定位信息'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	else 
		mSleep(2000);
	end



o = 10
	--repeat
	x, y = findColorInRegionFuzzy(0xff0000, 50, 39, 213, 145, 313); ---找性别
	if x ~= -1 and y ~= -1 then  
		c = x + 251
		
		touchDown(1, c, y);      --那么单击女
		mSleep(100);
		touchUp(1, c, y);
		mSleep(2000);
		
		x, y = findColorInRegionFuzzy(0xff0000, 50, 20, 308, 117, 410); --找地区
		if x ~= -1 and y ~= -1 then  --找地区
			c = x + 251
			
			touchDown(1, c, y);      --那么单击地区
			mSleep(100);
			touchUp(1, c, y);
			mSleep(5 * 1000);
			
			x, y = findColorInRegionFuzzy(0x50c229, 50, 8, 243, 225, 327); --找中国
			if x ~= -1 and y ~= -1 then  
				
				touchDown(1, x, y);      --那么单击中国
				mSleep(100);
				touchUp(1, x, y);
				mSleep(1000);
				x, y = findColorInRegionFuzzy(0xffffff, 90, 575, 70, 700, 130); --找下一步
				if x ~= -1 and y ~= -1 then  
					
					touchDown(1, x, y);      --那么单击中国
					mSleep(100);
					touchUp(1, x, y);
					mSleep(1000);
					o = o + 4
				else
					mSleep(1000);
					o = o + 2
				end
				o = o + 2
			else
				mSleep(100); 
			end
			
			o = o + 2
		else
			mSleep(100); 
		end
	o = o + 2
	else
		mSleep(100);
	end
--until(o>12)

	snapshot("pd.png", 11, 11, 12, 12);
	mSleep(2000);
	x, y = findColorInRegionFuzzy(0xffffff, 90, 210, 65, 285, 125); --找下一步
	if x ~= -1 and y ~= -1 then  
		
		z = 10
		repeat
			x, y = findImageInRegionFuzzy("pd.png", 95, 149, 965, 292, 1177, 0xffffff);  -- 判断是否翻到底 坐标
			if x ~= -1 and y ~= -1 then        --如果在指定区域找到某图片符合条件
				z = z + 2 --如果有pd.png图片 z+2 循环停止
				toast("添加附近的人完成",6); 
				mSleep(1038)
			else   
				x, y = findColorInRegionFuzzy(0x05295a, 90, 122, 146, 196, 273); --找下一步
				if x ~= -1 and y ~= -1 then  
					touchDown(1, x, y);      --那么单击中国
					mSleep(100);
					touchUp(1, x, y);
					mSleep(1800);
					
					
					--新版打招呼
					--[[
					x, y = findColorInRegionFuzzy(0x1aad19, 80, 166, 200, 194, 1158); --找绿色按钮
					if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
						
						tian = ocrText(x, y, x+200, y+100, 1);  --OCR 中文识别
						local t = type(string.find(tian, "添"));
						if t == "number" then    --有添字
							touchDown(1, x, y);      
							mSleep(100);
							touchUp(1, x, y);
							mSleep(3000);
							
							
							
							x, y = findColorInRegionFuzzy(0x1aad19, 80, 575, 65, 713, 135); --找绿色发送按钮
							if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
								inputText(hy2);          --写出字符串
								mSleep(1500);
								touchDown(1, 671, 97);      --发送
								mSleep(100);
								touchUp(1, 671, 97);
								mSleep(2000);
								
								for i = 1, 5 do  --返回至底层
									x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
									if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
									touchDown(1, 52, 97);      --返回
									mSleep(100);
									touchUp(1, 52, 97);
									mSleep(2000);
									end
								end
								
							else
								
								for i = 1, 5 do  --返回至底层
									x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
									if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
									touchDown(1, 52, 97);      --返回
									mSleep(100);
									touchUp(1, 52, 97);
									mSleep(2000);
									end
								end
								
							end
							
							
							
							
							
							
							
							
						else
							for i = 1, 5 do  --返回至底层
								x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
								if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
								touchDown(1, 52, 97);      --返回
								mSleep(100);
								touchUp(1, 52, 97);
								mSleep(2000);
								end
							end
						end
						
						
					else
						for i = 1, 5 do  --返回至底层
							x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
							if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
							touchDown(1, 52, 97);      --返回
							mSleep(100);
							touchUp(1, 52, 97);
							mSleep(2000);
							end
						end
					end
					]]
					
					
					
					x, y = findColorInRegionFuzzy(0x1aad19, 80, 157, 537, 240, 950); --打招呼
					if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
						
						touchDown(1, x, y);      --那么单击该点
						mSleep(100);
						touchUp(1, x, y);
						mSleep(1000);
						
						inputText(hy2);          --写出字符串
						mSleep(1500);
						touchDown(1, 671, 97);      --发送
						mSleep(100);
						touchUp(1, 671, 97);
						mSleep(1000);
						
						touchDown(1, 52, 97);      --返回
						mSleep(100);
						touchUp(1, 52, 97);
						mSleep(1000);
					else
						touchDown(1, 52, 97);      --返回
						mSleep(100);
						touchUp(1, 52, 97);
						mSleep(1000);
					end
					
					
				else
					--dialog("异常退出  04",3);
					jjb = "ERROR" 
					jjs = '异常退出'
					xxq = '未知'
					toast(jjs,3);
					snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
					file = io.open(rzlj,"a");
					file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
					file:close();
					goto jieshu
				end
				
				
				snapshot("pd.png", 169, 985, 272, 1157);
				mSleep(1000);
				touchDown(1,125,300)
				mSleep(12)
				touchMove(1,125,200)
				mSleep(1000);
				--snapshot("pd.png", 169, 985, 272, 1157);
			end
		until(z>11)
	else
		--dialog("找不到确认  失败",3);
		jjb = "ERROR" 
		jjs = '未找到"确认"按钮'
		xxq = '未知'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	end



	file = io.open(rzlj,"a");
	file:write("完成|"..hyjb.."|MESSAGE|无截图|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
	file:close();



goto jieshu

--------------------------6.群信息

::weixinqunxinxi::

txl = 0
repeat
	x, y = findColorInRegionFuzzy(0x46c01b, 80, w/4, h-250, w/2, h); --右上角。。。
	if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
		txl = 100
	else
		touchDown(1, w/2.7, h-50);      --返回
		mSleep(100);
		touchUp(1, w/2.7, h-50);
		mSleep(2000);
		txl = txl +1
	end
until(txl>3)

if tonumber(txl)<50 then
--dialog("无法点击通讯录",3);
jjb = "ERROR" 
jjs = '无法点击"通讯录"'
xxq = '未知'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
goto jieshu
end

ql = 0

repeat
	x, y = findColorInRegionFuzzy(0x45c01b, 98, 0, h/9, w/5, h/2); --绿色
	if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
		touchDown(1, x, y);      --点击群聊
		mSleep(100);
		touchUp(1, x, y);
		ql = ql + 1
		mSleep(2000);
	else
		ql = 100
	end
until(ql>5)

if tonumber(ql)<50 then
--dialog("无法进入群聊",3);
jjb = "ERROR" 
jjs = '无法点击"群聊"按钮'
xxq = '未知'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
goto jieshu
end
mSleep(1000);

	x, y = findColorInRegionFuzzy(0xffffff, 98, 0, h/6.4, 30, h/6.4+50); --是否有第一个群
	if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
		
	else
		--dialog("无微信群",3);
		jjb = "WARNING" 
		jjs = '无微信群'
		xxq = '无微信群 或 微信群未设置"保存到通讯录"'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	end

	x, y = findColorInRegionFuzzy(0xffffff, 98, 527*w0, 62*h0, 605*w0, 129*h0); --找放大镜
	if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
		touchDown(1, x, y);      --点击群聊
		mSleep(100);
		touchUp(1, x, y);
		mSleep(2000);
		inputText(hy4); 
		mSleep(4000);
	end
	
	x, y = findColorInRegionFuzzy(0xffffff, 98, 107*w0, 154*h0, 423*w0, 240*h0); --找放大镜
	if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
		touchDown(1, x, y);      --点击群聊
		mSleep(100);
		touchUp(1, x, y);
		mSleep(4000);
	else
		--dialog("无目标微信群",3);
		jjb = "WARNING" 
		jjs = '无目标微信群'
		xxq = '微信群标题关键字有误 或 微信群未设置"保存到通讯录"'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	end
	
	if tonumber(hy1) == 7 then
		goto jiaqunhaoyou
	end
	
	touchDown(1, 311*w0, 1233*h0);      --点击群聊
	mSleep(100);
	touchUp(1, 311*w0, 1233*h0);
	mSleep(3000);
	inputText(hy2);
	mSleep(800);

	touchDown(1, w-20, h-20);      --点击群聊
	mSleep(100);
	touchUp(1, w-20, h-20);
	




	file = io.open(rzlj,"a");
	file:write("完成|"..hyjb.."|MESSAGE|无截图|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
	file:close();






goto jieshu
--------------------------7.群好友
::jiaqunhaoyou::


		x, y = findColorInRegionFuzzy(0xffffff, 90, 643, 84, 696, 112); --找群设置
		if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
			touchDown(1, x, y);      --返回
			mSleep(100);
			touchUp(1, x, y);
			
		end
		

	mSleep(4000); 
	
	liao = ocrText(102, 54, 218, 145, 1);  --OCR 中文识别
	local t = type(string.find(liao, "聊"));
	if t == "number" then
		
	else
		--dialog("未在微信群内",3);
		jjb = "ERROR" 
		jjs = '未在微信群内'
		xxq = '未知'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		goto jieshu
	end


-- 进入群设置

zz = 0

repeat
	mSleep(1000);
	
	x0h, y0h = findColorInRegionFuzzy(0x353535, 90, 48*w0, 360*h0, 105*w0, 1248*h0); --找 黑字 群聊
	if x0h ~= -1 and y0h ~= -1 then  --如果在指定区域找到某点符合条件
		
		--dialog(x.."  "..y)  --48 646
		   --  301, 444, 388, 551
		x1 = x0h + 253*w0
		y1 = y0h - 202*h0
		x2 = x0h + 340*w0
		y2 = y0h - 95 *h0
		-- 47, 621, 106, 691  聊字坐标
		x0 = x0h + 58*w0
		y0 = y0h + 45*h0
		
		liao2 = ocrText(x0h, y0h, x0, y0, 1);
		local t = type(string.find(liao2, "聊"));
		if t == "number" then
			--toast("有聊字",2)
			
			quanbu = ocrText(x1, y1, x2, y2, 1);  --OCR 中文识别
			local t = type(string.find(quanbu, "全部"));
			if t == "number" then    --是否有全部 2字   
				touchDown(1, x1+50, y1+50);     --点击 全部2字
				mSleep(100);
				touchUp(1, x1+50, y1+50);
				mSleep(100);
				zz = 200
			else
				zz = 100
			end
			
		else --黑色不是聊字
			
			touchDown(1,125,300)   --下翻页
			mSleep(12)
			touchMove(1,125,200)
			
			zz = zz + 1
			
		end	
		
	else
		
		touchDown(1,125,300)   --下翻页
		mSleep(12)
		touchMove(1,125,200)
		zz = zz + 1
		
	end	
	
until(zz>20)

if tonumber(zz)<50 then
--dialog("群信息异常",3);
jjb = "WARNING" 
jjs = '群信息异常'
xxq = '未知错误.请重新尝试'
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
goto jieshu
end

if tonumber(zz) == 100 then
goto renshao
end


	mSleep(3000)
	
	--- 进入了聊天成员信息

for i = 1, 8 do  --上翻8次 返回顶端
touchDown(1,433,510)
mSleep(12)
touchMove(1,309,941)
mSleep(150)
	
end

	snapshot("pd.png", 171, 257, 297, 432); --截图  坐标
	ybs = 0
	xh = 0
	repeat
		if tonumber(ybs) < 4 then
			ybs = ybs + 1
		end  
		
		-- 466-286   =180
		y00 = ybs * 180 + 66
		
		x00, y01 = findColorInRegionFuzzy(0x999999, 90, 46, y00, 161, y00+176); --找 灰色名字
		if x00 ~= -1 and y01 ~= -1 then  --如果在指定区域找到某点符合条件
			--toast("建立坐标系 原点"..x00.."  "..y00)
			--66 402
			
			xbs = 0
			
			repeat
				x01 = x00 + xbs * 128
				
				
				touchDown(1, x01, y01);      
				mSleep(100);
				touchUp(1, x01, y01);
				mSleep(2000);
				----------
				
				
				x, y = findColorInRegionFuzzy(0x1aad19, 80, 166, 200, 194, 1158); --找绿色按钮
				if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
					
					tian = ocrText(x, y, x+200, y+100, 1);  --OCR 中文识别
					local t = type(string.find(tian, "添"));
					if t == "number" then    --有添字
						touchDown(1, x, y);      
						mSleep(100);
						touchUp(1, x, y);
						mSleep(3000);
						
						
						
						x, y = findColorInRegionFuzzy(0x7f7f7f, 90, 648, 71, 694, 128); --判断是否被拉黑
						if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
							
							mSleep(1000);
							x, y = findColorInRegionFuzzy(0x1aad19, 80, 475, 739, 600, 817); --判断是否被拉黑
							if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
								touchDown(1, x, y);      --点击确定
								mSleep(100);
								touchUp(1, x, y);
								mSleep(2000);
								
								
								for i = 1, 5 do  --返回至底层
									x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
									if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
									touchDown(1, 52, 97);      --返回
									mSleep(100);
									touchUp(1, 52, 97);
									mSleep(2000);
									end
								end
								
								
							end
							
							
							
							
						else
							
							
							
							
							x, y = findColorInRegionFuzzy(0x1aad19, 80, 575, 65, 713, 135); --找绿色发送按钮
							if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
								inputText(hy2);          --写出字符串
								mSleep(1500);
								touchDown(1, 671, 97);      --发送
								mSleep(100);
								touchUp(1, 671, 97);
								mSleep(2000);
								
								for i = 1, 5 do  --返回至底层
									x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
									if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
									touchDown(1, 52, 97);      --返回
									mSleep(100);
									touchUp(1, 52, 97);
									mSleep(2000);
									end
								end
								
							else
								
								for i = 1, 5 do  --返回至底层
									x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
									if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
									touchDown(1, 52, 97);      --返回
									mSleep(100);
									touchUp(1, 52, 97);
									mSleep(2000);
									end
								end
								
							end
							
						end
						
						
						
						
						
						
						
					else
						for i = 1, 5 do  --返回至底层
							x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
							if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
							touchDown(1, 52, 97);      --返回
							mSleep(100);
							touchUp(1, 52, 97);
							mSleep(2000);
							end
						end
					end
					
					
				else
					for i = 1, 5 do  --返回至底层
						x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
						if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
						touchDown(1, 52, 97);      --返回
						mSleep(100);
						touchUp(1, 52, 97);
						mSleep(2000);
						end
					end
				end
				
				
				
				
				xbs = xbs + 1
				
				
				
				
			until(xbs>=5)  --点完第5个出循环
			
			
			
			
			
		else	--找不到灰色名字
			
		end
		
		mSleep(1000);
		touchDown(1,125,330)   --下翻页
		mSleep(12)
		touchMove(1,125,200)
		mSleep(1000);
		xh = xh + 1
		
		x, y = findImageInRegionFuzzy("pd.png", 98, 161, 237, 307, 442, 0xffffff);  -- 判断是否翻到底 坐标
		if x ~= -1 and y ~= -1 then        --如果在指定区域找到某图片符合条件
			
			
			xh = 100
			dialog("群添加完成",3);
			
		else
			
			snapshot("pd.png", 171, 257, 297, 432); --截图  坐标
		end
		
	until(xh>20)
	
	file = io.open(rzlj,"a");
	file:write("完成|"..hyjb.."|MESSAGE|无截图|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
	file:close();
	
	goto jieshu

::renshao::

snapshot("pd.png", 171, 257, 297, 432); --截图  坐标

--x0h, y0h 聊 字坐标
yh = y0h - 412*h0 --灰色名字y轴初始坐标
ybs = 0 --y倍数  最多为3
xh = 0
xh2 = 0
repeat
	
	
	repeat
		
		if tonumber(xh2) > 0 then
			
			if tonumber(ybs) < 1 then
				ybs = ybs + 1
			end  
			
		end
		xh2 = xh2 + 1
		
		-- 466-286   =180
		y00 = yh - ybs * 180 * h0
		
		x00, y01 = findColorInRegionFuzzy(0x999999, 90, 46*w0, y00, 161*w0, y00+180); --找 灰色名字
		if x00 ~= -1 and y01 ~= -1 then  --如果在指定区域找到某点符合条件
			
			xbs = 0
			
			repeat
				x01 = x00 + xbs * 128
				
				
				touchDown(1, x01, y01);      
				mSleep(100);
				touchUp(1, x01, y01);
				mSleep(2000);
				----------
				
				
				x, y = findColorInRegionFuzzy(0x1aad19, 80, 166, 200, 194, 1158); --找绿色按钮
				if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
					
					tian = ocrText(x, y, x+200, y+100, 1);  --OCR 中文识别
					local t = type(string.find(tian, "添"));
					if t == "number" then    --有添字
						touchDown(1, x, y);      
						mSleep(100);
						touchUp(1, x, y);
						mSleep(3000);
						
						x, y = findColorInRegionFuzzy(0x7f7f7f, 80, 648, 71, 694, 128); --判断是否被拉黑
						if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
							
							mSleep(100);
							x, y = findColorInRegionFuzzy(0x1aad19, 80, 475, 739, 600, 817); --判断是否被拉黑
							if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
								touchDown(1, x, y);      --返回
								mSleep(100);
								touchUp(1, x, y);
								mSleep(2000);
								
								
								for i = 1, 5 do  --返回至底层
									x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
									if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
									touchDown(1, 52, 97);      --返回
									mSleep(100);
									touchUp(1, 52, 97);
									mSleep(2000);
									end
								end
								
							end
							
							
							
							
						else
							
							
							x, y = findColorInRegionFuzzy(0x1aad19, 80, 575, 65, 713, 135); --找绿色发送按钮
							if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
								inputText(hy2);          --写出字符串
								mSleep(1500);
								touchDown(1, 671, 97);      --发送
								mSleep(100);
								touchUp(1, 671, 97);
								mSleep(2000);
								
								for i = 1, 5 do  --返回至底层
									x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
									if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
									touchDown(1, 52, 97);      --返回
									mSleep(100);
									touchUp(1, 52, 97);
									mSleep(2000);
									end
								end
								
							else
								
								for i = 1, 5 do  --返回至底层
									x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
									if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
									touchDown(1, 52, 97);      --返回
									mSleep(100);
									touchUp(1, 52, 97);
									mSleep(2000);
									end
								end
								
							end
							
						end
						
						
						
					else
						for i = 1, 5 do  --返回至底层
							x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
							if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
							touchDown(1, 52, 97);      --返回
							mSleep(100);
							touchUp(1, 52, 97);
							mSleep(2000);
							end
						end
					end
					
					
				else
					for i = 1, 5 do  --返回至底层
						x, y = findColorInRegionFuzzy(0xffffff, 80, 662, 72, 684, 126); --右上角。。。
						if x ~= -1 and y ~= -1 then  --如果在指定区域找到某点符合条件
						touchDown(1, 52, 97);      --返回
						mSleep(100);
						touchUp(1, 52, 97);
						mSleep(2000);
						end
					end
				end
				
				
				
				
				xbs = xbs + 1
				
				
				
				
			until(xbs>=5)  --点完第5个出循环
			
			
		else -- 找不到灰色名字
			
		end
		
	until(ybs>=1)
	
	
	
		mSleep(1000);
		touchDown(1,125,200)   --上翻页
		mSleep(12)
		touchMove(1,125,328)
		mSleep(1000);
		xh = xh + 1
		
		x, y = findImageInRegionFuzzy("pd.png", 98, 161, 237, 307, 442, 0xffffff);  -- 判断是否翻到底 坐标
		if x ~= -1 and y ~= -1 then        --如果在指定区域找到某图片符合条件
			
			
			xh = 100
			dialog("群添加完成",3);
			
		else
			
			snapshot("pd.png", 171, 257, 297, 432); --截图  坐标
		end
		
until(xh>20)

	file = io.open(rzlj,"a");
	file:write("完成|"..hyjb.."|MESSAGE|无截图|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
	file:close();

goto jieshu

---------------------------
mSleep(5*1000);
::jieshu::





--[[

	ppp = getDeviceBrand()  --品牌
	xxh = getDeviceModel()  --机型
	ssj = os.time()   --本地时间
	ddl = batteryStatus()  --手机电量
	origin_ssj = os.date("%Y/%m/%d %X", os.time());  --格式化时间 2016/11/04 16:46:56

	
	if jb == 0 then
		jjs = "完成"
		jjb = "MESSAGE"
	elseif jb == 1 then
		jjb = "ERROR"
	elseif jb == 2 then
		jjb = "WARNING"
	end

	if jb == 0 then --完成
		file = io.open(rzlj,"a");
		file:write("完成|"..hyjb.."|MESSAGE|无截图|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		file:close();
		
	else --出错
		toast(jjs,3);
		snapshot(tplj..os.time()..".png", 0, 0, w-1, h-1);  -- 截图
		file = io.open(rzlj,"a");
		file:write(jjs.."|"..hyjb..":"..xxq.."|"..jjb.."|"..os.time()..".png|"..getDeviceBrand()..getDeviceModel().."|"..os.date("%Y/%m/%d %X", os.time()).."\n")
		--  格式 ：“简述|详情|级别”    一行一条。  级别有错误，警告和普通消息    错误时写ERROR。警告WARNING消息MESSAGE。不区分大小写
		--  无法发送图文朋友圈|在(0,0)到(720,1280)中找不到颜色0xffffff|error
		file:close();
	end
]]
	
	::guanbi::
	
	

	