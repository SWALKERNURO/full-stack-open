import { useEffect, useState } from 'react'
import { getCountries, getWeather } from './services/countries.js'

const describeWeather = code => {
  if (code === 0) return 'Clear sky'
  if ([1, 2, 3].includes(code)) return 'Partly cloudy or overcast'
  if ([45, 48].includes(code)) return 'Fog'
  if (code >= 51 && code <= 67) return 'Drizzle or rain'
  if (code >= 71 && code <= 77) return 'Snow'
  if (code >= 80 && code <= 82) return 'Rain showers'
  if ([85, 86].includes(code)) return 'Snow showers'
  if (code >= 95) return 'Thunderstorm'
  return 'Conditions unavailable'
}

const Weather = ({ country }) => {
  const [weather, setWeather] = useState(null)
  const [error, setError] = useState('')
  const coordinates = country.capitalInfo?.latlng

  useEffect(() => {
    if (!coordinates || coordinates.length < 2) return
    const controller = new AbortController()
    getWeather(coordinates, controller.signal).then(data => {
      if (!controller.signal.aborted) setWeather(data)
    }).catch(() => {
      if (!controller.signal.aborted) setError('Weather is temporarily unavailable.')
    })
    return () => controller.abort()
  }, [coordinates])

  if (!coordinates || coordinates.length < 2) return <p>Capital coordinates unavailable.</p>
  if (error) return <p role="alert">{error}</p>
  if (!weather) return <p>Loading weather.</p>
  if (!weather.current) return <p>Current conditions are unavailable.</p>
  return (
    <section aria-label="Capital weather">
      <h3>Weather in {country.capital?.[0] || 'the capital'}</h3>
      <p>temperature {weather.current.temperature_2m} {weather.current_units?.temperature_2m || '�C'}</p>
      <p>{describeWeather(weather.current.weather_code)}</p>
      <p>wind {weather.current.wind_speed_10m} {weather.current_units?.wind_speed_10m || 'km/h'}</p>
      <a href="https://open-meteo.com/">Weather data by Open-Meteo</a>
    </section>
  )
}

const Country = ({ country }) => (
  <section aria-label="Country details">
    <h2>{country.name.common}</h2>
    <p>capital {(country.capital || []).join(', ') || 'not listed'}</p>
    <p>area {country.area} km�</p>
    <h3>languages</h3>
    <ul>{Object.entries(country.languages || {}).map(([code, language]) => <li key={code}>{language}</li>)}</ul>
    <img src={country.flags.svg || country.flags.png} alt={country.flags.alt || `Flag of ${country.name.common}`} width="180" />
    <Weather key={country.cca3 || country.name.common} country={country} />
  </section>
)

const App = () => {
  const [countries, setCountries] = useState([])
  const [query, setQuery] = useState('')
  const [selected, setSelected] = useState(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const controller = new AbortController()
    getCountries(controller.signal).then(data => {
      if (!controller.signal.aborted) { setCountries(data); setLoading(false) }
    }).catch(() => {
      if (!controller.signal.aborted) { setError('Could not load countries. Please reload.'); setLoading(false) }
    })
    return () => controller.abort()
  }, [])

  const matches = countries.filter(country => country.name.common.toLowerCase().includes(query.trim().toLowerCase()))
  const country = selected || (matches.length === 1 ? matches[0] : null)

  return (
    <main>
      <h1>Countries</h1>
      <label>find countries <input value={query} onChange={event => { setQuery(event.target.value); setSelected(null) }} /></label>
      {loading ? <p>Loading countries.</p> : error ? <p role="alert">{error}</p> : country ? <Country country={country} /> :
        matches.length > 10 ? <p>Too many matches, specify another filter</p> : matches.length === 0 ? <p>No countries found</p> : (
          <ul>{matches.map(item => <li key={item.cca3 || item.name.common}>
            {item.name.common}{' '}<button onClick={() => setSelected(item)} aria-label={`show ${item.name.common}`}>show</button>
          </li>)}</ul>
        )}
    </main>
  )
}

export default App
