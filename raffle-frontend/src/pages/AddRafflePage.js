import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { raffleService } from '../services/api';
import './AddRafflePage.css';

const AddRafflePage = () => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState({
        title: '',
        description: '',
        imageUrl: '',
        ticketPrice: '',
        availableTickets: '',
        endDate: ''
    });
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');

        try {
            // Validare
            if (!formData.title || !formData.description || !formData.imageUrl ||
                !formData.ticketPrice || !formData.availableTickets || !formData.endDate) {
                throw new Error('Toate câmpurile sunt obligatorii');
            }

            const raffleData = {
                ...formData,
                ticketPrice: parseFloat(formData.ticketPrice),
                availableTickets: parseInt(formData.availableTickets)
            };

            await raffleService.createRaffle(raffleData);
            navigate('/raffles');
        } catch (err) {
            setError(err.message || 'A apărut o eroare la salvarea raflei');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="add-raffle-page">
            <h1>Adaugă o Nouă Raflă</h1>
            {error && <div className="error-message">{error}</div>}

            <form onSubmit={handleSubmit} className="raffle-form">
                <div className="form-group">
                    <label htmlFor="title">Titlu Mașină</label>
                    <input
                        type="text"
                        id="title"
                        name="title"
                        value={formData.title}
                        onChange={handleChange}
                        placeholder="Ex: BMW M3 Competition 2024"
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="description">Descriere</label>
                    <textarea
                        id="description"
                        name="description"
                        value={formData.description}
                        onChange={handleChange}
                        placeholder="Descrieți mașina, specificații, culoare, etc."
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="imageUrl">URL Imagine</label>
                    <input
                        type="url"
                        id="imageUrl"
                        name="imageUrl"
                        value={formData.imageUrl}
                        onChange={handleChange}
                        placeholder="https://example.com/image.jpg"
                    />
                </div>

                <div className="form-row">
                    <div className="form-group">
                        <label htmlFor="ticketPrice">Preț Bilet (RON)</label>
                        <input
                            type="number"
                            id="ticketPrice"
                            name="ticketPrice"
                            value={formData.ticketPrice}
                            onChange={handleChange}
                            min="1"
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="availableTickets">Număr de Bilete</label>
                        <input
                            type="number"
                            id="availableTickets"
                            name="availableTickets"
                            value={formData.availableTickets}
                            onChange={handleChange}
                            min="1"
                        />
                    </div>
                </div>

                <div className="form-group">
                    <label htmlFor="endDate">Data Încheierii</label>
                    <input
                        type="date"
                        id="endDate"
                        name="endDate"
                        value={formData.endDate}
                        onChange={handleChange}
                    />
                </div>

                <button type="submit" className="btn-submit" disabled={loading}>
                    {loading ? 'Se salvează...' : 'Adaugă Rafla'}
                </button>
            </form>
        </div>
    );
};

export default AddRafflePage; 