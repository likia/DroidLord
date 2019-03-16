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
x0 = tonumber(w) / 720
y0 = tonumber(h) / 1280
MyTable = {
    ["style"] = "default",
    ["width"] = w,
    ["height"] = h,
    ["config"] = "hyconfig.dat",
    ["timer"] = 3,
    views = {
        {
            ["type"] = "ComboBox",                       --下拉框，hy1
            ["list"] = "0.无,1.文字朋友圈,2.图文朋友圈,3.文字群发,4.通讯录好友,5.附近的人",--7个下拉选项，序号从0开始，即选项1编号为0，选项2编号为1，依此类推
            ["select"] = "0",                       --默认选择选项2
        },{
            ["type"] = "Edit",        --输入框，hy2
            ["prompt"] = "内容1",--编辑框中无任何内容时显示的底色文本
            ["text"] = "内容1",        --界面载入时已经存在于编辑框中的文本
        },{
            ["type"] = "ComboBox",                       --下拉框，hy3
            ["list"] = "1,2,3,4,5,6,7,8,9",--7个下拉选项，序号从0开始，即选项1编号为0，选项2编号为1，依此类推
            ["select"] = "0",                       --默认选择选项2
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
ret, hy1,hy2,hy3 = showUI(MyJsonString);--返回值ret,hy1 脚本类型 0=无 1=文字朋友圈   hy2 文字内容  hy3 群发图片数量

	if ret == 0 then
		goto jieshu
	end

	if tonumber(hy1) == 0 then
		goto jieshu
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





		toast(hyjb,5);
	end
	--------------------------启动微信APP
	closeApp("com.tencent.mm");  --关闭微信
	mSleep(2000);
	r = runApp("com.tencent.mm")--,"com.tencent.mm.plugin.nearby.ui.NearbyFriendsIntroUI") --启动微信并打开朋友圈
	toast("APP加载中");
	mSleep(10 * 1000);

	if r == 0 then


	else
		dialog("启动失败",3);
		goto jieshu

	end

	x, y = findColorInRegionFuzzy(0x46c01b, 98, x0*21, y0*1172, x0*683, y0*1277);  --检测是否在登录状态下
	if x ~= -1 and y ~= -1 then        --如果在指定区域找到某点符合条件
		toast("APP启动成功");
	else                               --如果找不到符合条件的点
		dialog("未登录微信帐号",3);
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
-----------------------------1.文字朋友圈
::wenzipengyouquan::

toast("文字朋友圈");
mSleep(100);
setScreenScale(true, 720, 1280)  --分辨路设置成720.1280
mSleep(100);
os.execute("am start com.tencent.mm/.plugin.sns.ui.SnsLongMsgUI") --启动微信并打开添加好友
toast("微信加载中");
mSleep(10 * 1000);

	--if r == 0 then
		toast("微信启动成功");

	-- else
	-- 	dialog("启动失败 错误01",3);
	-- 	goto jieshu
	-- end
mSleep(800);
----------------
	x, y = findColorInRegionFuzzy(0x1aad19, 90, 200, 1080, 530, 1230); --找发送  坐标
	if x ~= -1 and y ~= -1 then
		touchDown(1, x, y);      --那么单击发送
		mSleep(100);
		touchUp(1, x, y);
		toast("初始化文字朋友圈");
		mSleep(2000);

	else
		dialog("异常退出 错误02",3);
		goto jieshu
	end
-------------

	x, y = findColorInRegionFuzzy(0x2c6830, 90, 560, 55, 717, 145); --找发送  坐标
	if x ~= -1 and y ~= -1 then
		a = y + 100
		touchDown(1, x, a);
		mSleep(100);
		touchUp(1, x, a);
	else
		dialog("异常退出 错误05",3);
		goto jieshu
	end


mSleep(500);                --延迟 0.5 秒
toast(hy2);
inputText(hy2);                --写出字符串
mSleep(1500);

---------------

	x, y = findColorInRegionFuzzy(0x2c6830, 90, 560, 55, 717, 145); --找发送  坐标
	if x ~= -1 and y ~= -1 then
		dialog("发送内容为空 错误03",3);
		goto jieshu

	else
		x, y = findColorInRegionFuzzy(0x1aad19, 90, 560, 55, 717, 145); --找发送  坐标
		if x ~= -1 and y ~= -1 then
			touchDown(1, x, y);      --那么单击发送
			mSleep(100);
			touchUp(1, x, y);
			toast("发送成功");
			mSleep(2000);
		else
			dialog("异常退出 错误04",3);
			goto jieshu
		end
	end


goto jieshu
------------------------2.图文朋友圈
::tuwenpengyouquan::


	toast("图文朋友圈");
	mSleep(100);
	setScreenScale(true, 720, 1280)  --分辨路设置成720.1280
	mSleep(100);
	r = runApp("com.tencent.mm","com.tencent.mm.plugin.sns.ui.SnsTimeLineUI") --启动微信并打开添加好友
	toast("微信加载中");
	mSleep(10 * 1000);
	if r == 0 then
		toast("微信启动成功");

	else
		dialog("启动失败 错误01",3);
	end
	mSleep(800)
-------------------

c = 10
repeat
	x, y = findColorInRegionFuzzy(0xffffff, 95, x0*622, y0*66, x0*700, y0*130); --找右上角相机
	if x ~= -1 and y ~= -1 then
		touchDown(1, x, y);      --点击相机
		mSleep(100);
		touchUp(1, x, y);
		mSleep(100);
		toast("初始化图文朋友圈");
		mSleep(2000);

	else
		dialog("异常退出 错误02",3);
		goto jieshu
	end
	mSleep(2000);


	x, y = findColorInRegionFuzzy(0x6a6a6a, 90, x0*111, y0*595, x0*187, y0*640); --找 照片
	if x ~= -1 and y ~= -1 then
		touchDown(1, x, y);      --点击照片
		mSleep(100);
		touchUp(1, x, y);
		mSleep(3000);
	else

		dialog("异常退出 错误03",3);
		goto jieshu
	end
	mSleep(2000);
	x, y = findColorInRegionFuzzy(0xffffff, 99, x0*622, y0*66, x0*700, y0*130); --找发送  坐标
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
		x, y = findColorInRegionFuzzy(0x45c01a, 99, w0*2-w0/5 , h0 , w0*2-w0/5+30 , h0+30 ); --找发送  坐标
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
	until(tp1>12)

	if tonumber(tp1) < 100 then
		dialog("无法选择图片 1",3);
		goto jieshu
	end


	if tonumber(hy3) >= 1 then
		repeat
			x, y = findColorInRegionFuzzy(0x45c01a, 99, w0*3-w0/5 , h0 , w0*3-w0/5+30 , h0+30); --图片2  坐标
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
			dialog("无法选择图片 2",3);
			goto jieshu
		end
	end



	if tonumber(hy3) >= 2 then
		repeat
			x, y = findColorInRegionFuzzy(0x45c01a, 99,  w0*1-w0/5 , h0+w0 , w0*1-w0/5+30 , h0+30+w0); --图片3  坐标
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
			dialog("无法选择图片 3",3);
			goto jieshu
		end
	end


	if tonumber(hy3) >= 3 then
		repeat
			x, y = findColorInRegionFuzzy(0x45c01a, 99,  w0*2-w0/5 , h0+w0 , w0*2-w0/5+30 , h0+30+w0); --图片4  坐标
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
			dialog("无法选择图片 4",3);
			goto jieshu
		end
	end




	if tonumber(hy3) >= 4 then

		repeat
			x, y = findColorInRegionFuzzy(0x45c01a, 99, w0*3-w0/5 , h0+w0 , w0*3-w0/5+30 , h0+30+w0); --图片5  坐标
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
			dialog("无法选择图片 5",3);
			goto jieshu
		end
	end





	if tonumber(hy3) >= 5 then
		repeat
			x, y = findColorInRegionFuzzy(0x45c01a, 99, w0*1-w0/5 , h0+w0*2 , w0*1-w0/5+30 , h0+30+w0*2); --图片6  坐标
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
			dialog("无法选择图片 6",3);
			goto jieshu
		end
	end





	if tonumber(hy3) >= 6 then
		repeat
			x, y = findColorInRegionFuzzy(0x45c01a, 99,w0*2-w0/5 , h0+w0*2 , w0*2-w0/5+30 , h0+30+w0*2); --图片7  坐标
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
			dialog("无法选择图片 7",3);
			goto jieshu
		end
	end



	if tonumber(hy3) >= 7 then
		repeat
			x, y = findColorInRegionFuzzy(0x45c01a, 99, w0*3-w0/5 , h0+w0*2 , w0*3-w0/5+30 , h0+30+w0*2); --图片8  坐标
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
			dialog("无法选择图片 8",3);
			goto jieshu
		end
	end


	if tonumber(hy3) >= 8 then
		repeat
			x, y = findColorInRegionFuzzy(0x45c01a, 99,w0*1-w0/5 , h0+w0*3 , w0*1-w0/5+30 , h0+30+w0*3); --图片9  坐标
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
			dialog("无法选择图片 9",3);
			goto jieshu
		end

	end













	x, y = findColorInRegionFuzzy(0x1aad19, 90, 600, 60, 707, 130); --找完成  坐标
if x ~= -1 and y ~= -1 then
	touchDown(1, x, y);      --那么单击发送
	mSleep(100);
    touchUp(1, x, y);
	mSleep(4000);
else

    dialog("异常退出 错误05",3);
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

    dialog("异常退出 错误06",3);
	goto jieshu
end




mSleep(500);                --延迟 0.5 秒
inputText(hy2);          --写出字符串
mSleep(1500);

	x, y = findColorInRegionFuzzy(0x2c6830, 90, 560, 55, 717, 145); --找发送  坐标
	if x ~= -1 and y ~= -1 then
		dialog("发送内容为空 错误03",3);
		goto jieshu

	else
		x, y = findColorInRegionFuzzy(0x1aad19, 90, 560, 55, 717, 145); --找发送  坐标
		if x ~= -1 and y ~= -1 then
			touchDown(1, x, y);      --那么单击发送
			mSleep(100);
			touchUp(1, x, y);
			toast("发送成功");
			mSleep(2000);
		else
		 dialog("异常退出 错误04",3);
			goto jieshu
		end
	end

goto jieshu
---------------------------3.文字群发
::wenziqunfa::

r = runApp("com.tencent.mm","com.tencent.mm.plugin.masssend.ui.MassSendSelectContactUI") --启动微信并打开添加好友
toast("微信加载中");
mSleep(10 * 1000);

	if r == 0 then
		toast("微信启动成功");

	else
		dialog("启动失败 错误01",3);
	end
mSleep(800);
-----------------
e = 10
	repeat  --
		x, y = findColorInRegionFuzzy(0x2c6830, 90, 590, 57, 717, 145); --找下一步  坐标
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
				dialog("异常退出 错误02",3);
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
				dialog("异常退出 错误03",3);
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
		dialog("异常退出 错误05",3);
		goto jieshu
	end
-----------------------------------
	x, y = findColorInRegionFuzzy(0x1aad19, 90, 230, 1150, 505, 1280); --找发送  坐标
	if x ~= -1 and y ~= -1 then

		toast("发送成功");
	else
		dialog("异常退出 错误06",3);
		goto jieshu
	end
goto jieshu
-------------------------------4.批量添加通讯录好友
::tongxunlu::

toast("添加通讯录脚本开始");
mSleep(100);
setScreenScale(true, 720, 1280)  --分辨路设置成7200.12800
mSleep(100);



r = runApp("com.tencent.mm","com.tencent.mm.plugin.subapp.ui.pluginapp.AddMoreFriendsByOtherWayUI") --启动微信并打开添加好友
toast("微信加载中");
mSleep(10 * 1000);

	if r == 0 then
		toast("微信启动成功");

	else
		dialog("启动失败",3);
	end
mSleep(800);
---------------------------------------------

	x, y = findColorInRegionFuzzy(0x4ec326, 80, 15, 166, 109, 407); --找确定  坐标
	if x ~= -1 and y ~= -1 then  --找添加手机联系人

		touchDown(1, x, y);      --那么单击添加手机联系人
		mSleep(100);
		touchUp(1, x, y);
		toast("添加手机联系人");
	else
		dialog("异常退出",3);
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
					dialog("异常退出",3);
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
		x, y = findImageInRegionFuzzy("判断.png", 90, 149, 965, 292, 1177, 0xffffff);  -- 判断是否翻到底 坐标
		if x ~= -1 and y ~= -1 then        --如果在指定区域找到某图片符合条件
		  z = z + 2 --如果有判断.png图片 z+2 循环停止
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


		   snapshot("判断.png", 169, 985, 272, 1157); --截图  坐标
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





goto jieshu
-------------------------5.附近的人
::fujinderen::



toast("添加身边的人脚本开始");
setScreenScale(true, 720, 1280)
mSleep(1000);


r = runApp("com.tencent.mm","com.tencent.mm.plugin.nearby.ui.NearbyFriendsIntroUI") --启动微信并打开朋友圈
toast("APP加载中");
mSleep(10 * 1000);

	if r == 0 then
		toast("APP启动成功");

	else
		dialog("启动失败",3);
	end

mSleep(1000);


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
			dialog("找不到确认  失败",3);
		end
		-----------------------------
	else                         --如果找不到符合条件的点
		toast("初始化完成",0);
	end
mSleep(1000);

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



	x, y = findColorInRegionFuzzy(0xffffff, 90, 210, 65, 285, 125); --找下一步
	if x ~= -1 and y ~= -1 then

		z = 10
		repeat
			x, y = findImageInRegionFuzzy("判断.png", 90, 149, 965, 292, 1177, 0xffffff);  -- 判断是否翻到底 坐标
			if x ~= -1 and y ~= -1 then        --如果在指定区域找到某图片符合条件
				z = z + 2 --如果有判断.png图片 z+2 循环停止
				toast("添加附近的人完成");
				mSleep(1038)
			else
				x, y = findColorInRegionFuzzy(0x05295a, 90, 122, 146, 196, 273); --找下一步
				if x ~= -1 and y ~= -1 then
					touchDown(1, x, y);      --那么单击中国
					mSleep(100);
					touchUp(1, x, y);
					mSleep(1800);
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
					dialog("异常退出  04",3);
				end


				snapshot("判断.png", 169, 985, 272, 1157);
				mSleep(1000);
				touchDown(1,125,273)
				mSleep(12)
				touchMove(1,125,145)
				mSleep(16)
			end
		until(z>11)
	else
		dialog("找不到确认  失败",3);
	end







goto jieshu
--------------------------
mSleep(5*1000);
::jieshu::
