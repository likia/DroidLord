local socket = require "szocket.core"
local client = socket.tcp

tcp:connect("127.0.0.1", 1989)
tcp:send("123456789\0")