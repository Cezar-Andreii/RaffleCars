import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import HomePage from './pages/HomePage';
import RafflesPage from './pages/RafflesPage';
import WinnersPage from './pages/WinnersPage';
import AddRafflePage from './pages/AddRafflePage';
import './App.css';

function App() {
  return (
    <Router>
      <div className="App">
        <Navbar />
        <main className="main-content">
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/raffles" element={<RafflesPage />} />
            <Route path="/winners" element={<WinnersPage />} />
            <Route path="/add-raffle" element={<AddRafflePage />} />
          </Routes>
        </main>
      </div>
    </Router>
  );
}

export default App;
