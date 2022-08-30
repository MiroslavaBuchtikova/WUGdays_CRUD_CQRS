function list_child_processes(){
    local ppid=$1;
    local current_children=$(pgrep -P $ppid);
    local local_child;
    if [ $? -eq 0 ];
    then
        for current_child in $current_children
        do
          local_child=$current_child;
          list_child_processes $local_child;
          echo $local_child;
        done;
    else
      return 0;
    fi;
}

ps 18399;
while [ $? -eq 0 ];
do
  sleep 1;
  ps 18399 > /dev/null;
done;

for child in $(list_child_processes 18405);
do
  echo killing $child;
  kill -s KILL $child;
done;
rm /Users/buchtikova/RiderProjects/Projects/WUGdays_University_CRUD_CQRS/University_CQRS/University/bin/Debug/net6.0/8eca2a27d8c8413b8bc145ea569f4b0a.sh;
