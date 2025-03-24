import React from 'react';
import './WinnersPage.css';

const WinnersPage = () => {
    // Aici vom adăuga logica pentru a prelua câștigătorii de la backend
    const winners = [
        {
            id: 1,
            name: "Ioan Popescu",
            prize: "BMW M3 Competition",
            date: "15 Martie 2024",
            ticketNumber: "A123456"
        },
        {
            id: 2,
            name: "Maria Ionescu",
            prize: "Audi RS6 Avant",
            date: "1 Martie 2024",
            ticketNumber: "B789012"
        }
    ];

    return (
        <div className="winners-page">
            <h1>Câștigătorii Noștri</h1>
            <div className="winners-grid">
                {winners.map((winner) => (
                    <div key={winner.id} className="winner-card">
                        <div className="winner-info">
                            <h3>{winner.name}</h3>
                            <p className="prize">{winner.prize}</p>
                            <div className="winner-details">
                                <span>Data: {winner.date}</span>
                                <span>Bilet: {winner.ticketNumber}</span>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
};

export default WinnersPage; 