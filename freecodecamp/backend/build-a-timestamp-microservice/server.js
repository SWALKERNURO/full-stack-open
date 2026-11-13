import express from "express";
import { rateLimit } from "express-rate-limit";
import cors from "cors";

const app = express();
// Bound per-process request work before parsing bodies, authorization or database access.
app.use(rateLimit({
  windowMs: 60_000,
  limit: 1000,
  standardHeaders: 'draft-8',
  legacyHeaders: false,
  message: { error: 'Too many requests; retry shortly.' },
}));

app.use(cors({ optionsSuccessStatus: 200 }));

app.use(express.static("public"));

app.get("/", (_req, res) => {
  res.sendFile(import.meta.dirname + "/views/index.html");
});

// Do not change code above this line

function timestampResponse(req, res) {
  const input = req.params.date;
  const date = input === undefined ? new Date()
    : /^-?\d+$/.test(input) ? new Date(Number(input)) : new Date(input);
  if (Number.isNaN(date.getTime())) return res.json({ error: "Invalid Date" });
  return res.json({ unix: date.getTime(), utc: date.toUTCString() });
}
app.get("/api", timestampResponse);
app.get("/api/:date", timestampResponse);

// Do not change code below this line

const PORT = 8000;
const listener = app.listen(PORT, function () {
  console.log("Your app is listening on port " + listener.address().port);
});
