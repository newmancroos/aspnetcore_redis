# aspnetcore_redis

<ul>
<li>Download redis docker image <br>
    <pre>
        docker pull redis
    </pre>
</li>
<li>
    run docker container locally <br>
    <pre>
        docker run --name local-redis -p 6379:6379 -d redis
    </pre>
</li>
<li>
    Check docker container is running<br>
    <pre>
        docker ps
    </pre>
</li>
<li>
    check docker log <br>
    <pre>
        docker logs -f local-redis
    </pre>
</li>
<li>
    Open redis CLI inside the container execute redis <br>
    <pre>
        docker exec -it local-redis /bin/bash    and then type
        redis-cli 
    </pre>
</li>
<li>
    Check redis response well by typeing the command
    <pre>
        ping
    </pre>
    so that redis reply with PONG
</li>
<li>
    Set a Key and Value in the redis cache this is how it works in side the c# code too<br>
    <i>set key value</i>
    <pre>
        set mykey Test
    </pre>
    and we can get the value of the key
    <pre>
        get mykey
    </pre>
</li>
</ul>

Same way within aspo controller we need to add redis nuget package and in startup

<pre>
                var redis = ConnectionMultiplexer.Connect("localhost");
            services.AddScoped<IDatabase>(s =&gt; redis.GetDatabase());
</pre>

We can inject IDatabase and use <b>database.StringGet(key></b> and <b>database.StringSet(Key, value)</b>
