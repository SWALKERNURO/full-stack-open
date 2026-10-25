const { useState, useMemo } = React;
const RATES = { USD: 1, EUR: 0.92, GBP: 0.78, JPY: 156.7 };

export function CurrencyConverter() {
  const [amount, setAmount] = useState('100');
  const [from, setFrom] = useState('USD');
  const [to, setTo] = useState('EUR');
  const convertedAmounts = useMemo(() => {
    const base = (Number(amount) || 0) / RATES[from];
    return Object.fromEntries(Object.entries(RATES).map(([code, rate]) => [code, base * rate]));
  }, [amount, from]);
  const currencies = Object.keys(RATES);
  return <main className="converter"><p className="eyebrow">POCKET EXCHANGE / A QUICK CONVERSION</p><h1>A different currency.<br />The same idea.</h1><label htmlFor="amount">Amount</label><input id="amount" type="number" min="0" step="any" value={amount} onChange={event => setAmount(event.target.value)} /><div className="currencies"><div><label htmlFor="from">From</label><select id="from" value={from} onChange={event => setFrom(event.target.value)}>{currencies.map(code => <option key={code} value={code}>{code}</option>)}</select></div><span aria-hidden="true">→</span><div><label htmlFor="to">To</label><select id="to" value={to} onChange={event => setTo(event.target.value)}>{currencies.map(code => <option key={code} value={code}>{code}</option>)}</select></div></div><p className="result" role="status" aria-live="polite">{convertedAmounts[to].toFixed(2)} {to}</p><p className="note">Illustrative fixed rates for a learning project. These are not live market rates.</p></main>;
}