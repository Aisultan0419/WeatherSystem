let updateIntervalId = null;
let connection = null;
let currentSubscription = null; 



document.addEventListener('DOMContentLoaded', () => {
  const button = document.querySelector('#showButton');

  button.addEventListener('click', async () => {
    const citySelect = document.querySelector('select[name="city"]');
    const strategyInput = document.querySelector('input[name="strategy"]:checked');

    if (!citySelect || !strategyInput) {
      console.error('Some elements were not found.');
      return;
    }

    const city = citySelect.value;
    const strategy = strategyInput.value;

    await updateWeather(city, strategy);

    if (strategy === 'RealTime') {
      if (updateIntervalId) clearInterval(updateIntervalId);
      updateIntervalId = setInterval(() => updateWeather(city, strategy), 2000);
    } 
    else if (strategy === 'Batch') {
      if (updateIntervalId) clearInterval(updateIntervalId);
      updateIntervalId = setInterval(() => updateWeather(city, strategy), 1800000);
    }
    else {
      if (updateIntervalId) {
        clearInterval(updateIntervalId);
        updateIntervalId = null;
      }
    }
  });
});


async function getData(city, strategy) {
  try {
    const res = await fetch(`https://localhost:7149/api/WeatherDisplay?city=${city}&strategy=${strategy}`);
    if (!res.ok) throw new Error(`HTTP error occurred: ${res.status}`);
    return await res.json();
  } catch (err) {
    console.error('Error:', err);
    return { error: err.message };
  }
}


async function updateWeather(city, strategy) {
  const data = await getData(city, strategy);
  if (data.error) {
    console.error("Error fetching data:", data.error);
    return;
  }

  const cityEl = document.querySelector('.CityPlace');
  const humEl = document.querySelector('.humidity');
  const tempEl = document.querySelector('.tempRate');
  const desEl = document.querySelector('.description');
  const timeEl = document.querySelector('.time');

  cityEl.textContent = data.city;
  humEl.textContent = `${data.humidity}%`;
  tempEl.textContent = `${data.temperatureC.toFixed(1)}°C`;
  desEl.textContent = data.summary;

  const date = new Date(data.timestamp);
  const formatted = date.toLocaleString("en-US", {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
    second: "2-digit",
    hour12: true,
    timeZoneName: "short"
  });
  timeEl.textContent = formatted;
}



document.addEventListener('DOMContentLoaded', () => {
  const subscribeBtn = document.querySelector('#tryButton');

subscribeBtn.addEventListener('click', async () => {
  const observerInput = document.querySelector('input[name="observer"]:checked');
  const citySelect = document.querySelector('select[name="city"]');

  if (!observerInput || !citySelect) {
    console.error('Observer or city not selected');
    return;
  }

  const method = observerInput.value; 
  const city = citySelect.value;
  const weather = await getData(city, 'RealTime');
  if (weather.error) {
    console.error('Failed to get weather for subscription:', weather.error);
    return;
  }
  const subscriber = {
    Name: "User1",
    Method: method,
    City: city,
    ConnectionId: null
  };
  if (method.toLowerCase() === 'web') {
    if (!connection) {
      connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7149/weatherHub")
        .withAutomaticReconnect()
        .build();
      connection.on("WeatherUpdated", (data) => {
        console.log("✅ Weather updated via WebSocket:", data);
        updateWeatherUI(data);
      });
      connection.onreconnected(async (newId) => {
        console.log("🔄 SignalR reconnected, new id:", newId);
        if (currentSubscription) {
          currentSubscription.subscriber.ConnectionId = newId;
          try {
            await fetch('https://localhost:7149/api/subscribers', {
              method: 'POST',
              headers: { 'Content-Type': 'application/json' },
              body: JSON.stringify(currentSubscription)
            });
            console.log('Re-subscribed after reconnect');
          } catch (err) {
            console.error('Failed to re-subscribe:', err);
          }
        }
      });

      try {
        await connection.start();
        console.log('Connected to SignalR hub, id =', connection.connectionId);
      } catch (err) {
        console.error('SignalR connection error:', err);
        return;
      }
    }
    subscriber.ConnectionId = connection.connectionId;
  }

  const requestBody = {
    subscriber: subscriber,
    weather: weather
  };

  console.log('📦 Subscription request body:', JSON.stringify(requestBody, null, 2));

  try {
    const response = await fetch('https://localhost:7149/api/subscribers', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(requestBody)
    });

    if (!response.ok) {
      const errorText = await response.text();
      console.error('❌ Subscribe failed:', response.status, errorText);
      return;
    }

    currentSubscription = requestBody; 
    console.log(`📡 Subscribed for ${method} updates in ${city}`);
  } catch (err) {
    console.error('Subscribe API error:', err);
  }
});


});


async function sendSubscription(dto) {
  try {
    const res = await fetch('https://localhost:7149/api/subscribers', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(dto)
    });

    if (!res.ok) {
      throw new Error(`Subscribe API error: ${res.status}`);
    }

    console.log(`Subscribed for ${dto.Method} updates in ${dto.City}`);
  } catch (err) {
    console.error("Subscribe API error:", err);
  }
}
