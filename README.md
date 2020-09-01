# aspnetcore_redis

<h4>This Branch also have other Caching models.</h4>
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
<b>
    Install Nuget package StackExchange.Redis
</b>
Same way within aspo controller we need to add redis nuget package and in startup

<pre>
                var redis = ConnectionMultiplexer.Connect("localhost");
            services.AddScoped<IDatabase>(s =&gt; redis.GetDatabase());
</pre>

We can inject IDatabase and use <b>database.StringGet(key></b> and <b>database.StringSet(Key, value)</b>

<hr/>
<h2>In-Memory Cache</h2>
<br>
<p>
    In-Memory cache is good for single server not for multiple server and load balancing. <br>
    Caching works best with data that changes infrequently and is expensive to generate. Caching makes a copy of data that can be returned much faster than from the source.<br>
    ASP.NET Core supports several different caches. The simplest cache is based on the IMemoryCache. IMemoryCache represents a cache stored in the memory of the web server. Apps running on a server farm (multiple servers) should ensure sessions are sticky when using the in-memory cache. Sticky sessions ensure that subsequent requests from a client all go to the same server. For example, Azure Web apps use Application Request Routing (ARR) to route all subsequent requests to the same server.<br>
    Non-sticky sessions in a web farm require a distributed cache to avoid cache consistency problems. For some apps, a distributed cache can support higher scale-out than an in-memory cache. Using a distributed cache offloads the cache memory to an external process.<br>
    The in-memory cache can store any object. The distributed cache interface is limited to byte[]. The in-memory and distributed cache store cache items as key-value pairs.
    <br>
        <h3>Cache Guidline</h3><br>
        <ol>
            <li>
                Code should always have a fallback option to fetch data and not depend on a cached value being available.
            </li>
            <li>
                The cache uses a scarce resource, memory. Limit cache growth:<br>
                    <ul>
                        <li>
                            Do not use external input as cache keys.
                        </li>
                        <li>
                            Use expirations to limit cache growth.
                        </li>
                        <li>
                            Use SetSize, Size, and SizeLimit to limit cache size. The ASP.NET Core runtime does not limit cache size based on memory pressure. It's up to the developer to limit cache size
                        </li>
                    </ul>
            </li>
        </ol>
</p>
<p>
    Install Nueget packge "Microsoft.Extensions.Caching.Memory" and "Microsost.Extensions.Caching.Abstractions" <br>
</p>
<p>
    Redis as another two very importent facilities
        <ol>
            <li>
                <b>Subscriber</b> - When we create a subscriber that will execute a method when ever it receive a message to the cache chennel.
            </li>
            <li>
                <b>Replica</b> - We can create a Redis cache replica database so that when ever we change the cache a replica copy will be created in the replica database
            </li>
        </ol>
</p>
<p>
    Read more on In-Memory cache before useing it.<br>
    <a href="https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.1">In-Memory Cache</a>
</p>
