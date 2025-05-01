const express = require('express');
const cors = require('cors');
const bodyParser = require('body-parser');
const cookieParser = require('cookie-parser');
const sqlite3 = require('sqlite3').verbose();

const app = express();
const PORT = 3000;

app.use(cors());
app.use(bodyParser.json());
app.use(cookieParser());

// Create SQLite database
const db = new sqlite3.Database('./users.db', (err) => {
  if (err) console.error(err.message);
  else console.log('Connected to users database.');
});

// Create users table
db.run(`CREATE TABLE IF NOT EXISTS users (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  username TEXT,
  email TEXT UNIQUE,
  password TEXT,
  score INTEGER DEFAULT 0
)`);


// 1. Register user
app.post('/register', (req, res) => {
  const { username, email, password } = req.body;
  const sql = `INSERT INTO users (email, password) VALUES (?, ?)`;

  db.run(sql, [email, password], function (err) {
    if (err) return res.status(400).json({ error: err.message });
    res.json({ id: this.lastID, email });
  });
});
// Login user
app.post('/login', (req, res) => {
  const { email, password } = req.body;
  const sql = `SELECT * FROM users WHERE email = ? AND password = ?`;

  db.get(sql, [email, password], (err, user) => {
    if (err) return res.status(400).json({ error: err.message });
    if (!user) return res.status(401).json({ error: 'Invalid email or password' });
    res.json({ message: 'Login successful', user });
  });
});


// 2. Get user by ID
app.get('/user/:id', (req, res) => {
  const sql = `SELECT * FROM users WHERE id = ?`;
  db.get(sql, [req.params.id], (err, row) => {
    if (err) return res.status(400).json({ error: err.message });
    res.json(row);
  });
});

// 3. Update score
app.put('/update-score', (req, res) => {
  const { id, score } = req.body;
  const sql = `UPDATE users SET score = ? WHERE id = ?`;
  db.run(sql, [score, id], function (err) {
    if (err) return res.status(400).json({ error: err.message });
    res.json({ message: 'Score updated successfully' });
  });
});

// 4. Delete user
app.delete('/user/:id', (req, res) => {
  const sql = `DELETE FROM users WHERE id = ?`;
  db.run(sql, [req.params.id], function (err) {
    if (err) return res.status(400).json({ error: err.message });
    res.json({ message: 'User deleted' });
  });
});

app.listen(PORT, () => {
  console.log(`Server running on http://localhost:${PORT}`);
});
