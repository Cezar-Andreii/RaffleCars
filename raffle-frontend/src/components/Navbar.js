import React from 'react';
import { Link } from 'react-router-dom';
import './Navbar.css';

const Navbar = () => {
    return (
        <nav className="navbar">
            <div className="navbar-brand">
                <Link to="/">RaffleCars</Link>
            </div>
            <div className="navbar-links">
                <Link to="/">Acasă</Link>
                <Link to="/raffles">Rafle Active</Link>
                <Link to="/winners">Câștigători</Link>
                <Link to="/add-raffle" className="btn-add-raffle">Adaugă Raflă</Link>
            </div>
            <div className="navbar-auth">
                <Link to="/login" className="btn-login">Autentificare</Link>
                <Link to="/register" className="btn-register">Înregistrare</Link>
            </div>
        </nav>
    );
};

export default Navbar; 