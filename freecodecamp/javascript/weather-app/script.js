const citySelect = document.getElementById('city');
const weatherButton = document.getElementById('get-weather-btn');
const weatherCard = document.getElementById('weather-card');
const requestStatus = document.getElementById('request-status');

var getWeather = async function (city) {
  try {
    const response = await fetch('https://weather-proxy.freecodecamp.rocks/api/city/' + encodeURIComponent(city));
    if (response.ok === false) throw new Error('Weather request failed: ' + response.status);
    return await response.json();
  } catch (error) {
    console.error(error);
    return undefined;
  }
}

var showWeather = async function (city) {
  if (!city) return;
  const data = await getWeather(city);
  if (!data) {
    weatherCard.hidden = true;
    alert('Something went wrong, please try again later');
    return;
  }
  const values = {
    'main-temperature': data.main?.temp,
    'feels-like': data.main?.feels_like,
    humidity: data.main?.humidity,
    wind: data.wind?.speed,
    'wind-gust': data.wind?.gust,
    'weather-main': data.weather?.[0]?.main,
    location: data.name
  };
  for (const [id, value] of Object.entries(values)) {
    document.getElementById(id).textContent = value ?? 'N/A';
  }
  const icon = document.getElementById('weather-icon');
  icon.src = data.weather?.[0]?.icon ?? 'N/A';
  icon.alt = data.weather?.[0]?.description ?? 'N/A';
  icon.hidden = !data.weather?.[0]?.icon;
  weatherCard.hidden = false;
}
weatherButton.addEventListener('click', async () => {
  const city = citySelect.value;
  if (!city) return;
  requestStatus.textContent = 'Checking the sky…';
  weatherButton.disabled = true;
  try {
    await showWeather(city);
  } finally {
    requestStatus.textContent = '';
    weatherButton.disabled = false;
  }
});