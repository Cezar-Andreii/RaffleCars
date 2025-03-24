import React, { useState, useEffect } from 'react';
import { raffleService } from '../services/api';
import './RafflesPage.css';

const RafflesPage = () => {
    const [raffles, setRaffles] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchRaffles = async () => {
            try {
                const data = await raffleService.getAllRaffles();
                setRaffles(data);
            } catch (err) {
                setError('A apărut o eroare la încărcarea raflelor.');
                console.error('Error fetching raffles:', err);
            } finally {
                setLoading(false);
            }
        };

        fetchRaffles();
    }, []);

    if (loading) return <div className="loading">Se încarcă...</div>;
    if (error) return <div className="error">{error}</div>;

    return (
        <div className="raffles-page">
            <h1>Rafle Active</h1>
            <div className="raffles-grid">
                {raffles.map((raffle) => (
                    <div key={raffle.id} className="raffle-card">
                        <img src={raffle.imageUrl} alt={raffle.title} />
                        <div className="raffle-info">
                            <h3>{raffle.title}</h3>
                            <p>{raffle.description}</p>
                            <div className="raffle-details">
                                <span>Preț bilet: {raffle.ticketPrice} RON</span>
                                <span>Bilete disponibile: {raffle.availableTickets}</span>
                            </div>
                            <button className="btn-participate">Participă acum</button>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default RafflesPage; 