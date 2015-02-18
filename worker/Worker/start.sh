set -e
set -u

echo "start.sh NUMOFWORKERS ONLY_WORKER_OR_MASTER(m OR w)"

cp /home/langus0/repo/XnivelMattReduce/worker/Worker/ExampleMapper/bin/Debug/ExampleMapper.dll /home/langus0/repo/XnivelMattReduce/worker/Worker/ExampleMapper.dll
numOfWorkers=$1
echo "Workers $numOfWorkers"
if [ $numOfWorkers -gt 0 ]; then
rm -fr /tmp/Worker-*
for i in `seq $numOfWorkers`; do
	cp -R  Worker/bin/Debug/ /tmp/Worker-$i
	echo "<?xml version=\"1.0\" encoding=\"utf-8\"?>
<configuration>
        <appSettings>
        <add key=\"WorkingDirectory\" value=\"/home/langus0/git/XnivelMattReduce/workdir/$i\"/>
    </appSettings>
</configuration>
" > /tmp/Worker-$i/Worker.exe.config 
done
fi

type=$2
if [ "$type" == "m" -o "$type" == "a" ]; then
xterm -e "mono Master/bin/Debug/Master.exe" &
fi

if [ "$type" == "w" -o "$type" == "a" ]; then
	if [ $numOfWorkers -gt 0 ]; then
		for i in `seq $numOfWorkers`; do
			echo "mono /tmp/Worker-$i/Worker.exe http://127.0.0.1:$((1336+$i))/"
			xterm -e "mono /tmp/Worker-$i/Worker.exe http://127.0.0.1:$((1336+$i))/; read -n1 kbd" &
		done
	else
		xterm -e "mono Worker/bin/Debug/Worker.exe"
	fi
fi
