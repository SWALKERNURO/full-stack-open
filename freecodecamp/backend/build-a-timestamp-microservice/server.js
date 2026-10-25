import express from "express";
import cors from "cors";

const app = express();

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
