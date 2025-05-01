// database.js
const sqlite3 = require('sqlite3').verbose();
const db = new sqlite3.Database('./users.db');

// Create table if it doesn't exist
db.serialize(() => {
  db.run(`CREATE TABLE IF NOT EXISTS users (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    username TEXT NOT NULL,
    email TEXT NOT NULL,
    score INTEGER DEFAULT 0
  )`);
});

module.exports = db;
