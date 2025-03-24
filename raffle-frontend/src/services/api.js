import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL || 'https://localhost:7000/api';

const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Date de test pentru dezvoltare
const TEST_DATA = {
    raffles: [
        {
            id: 1,
            title: "BMW M3 Competition 2024",
            description: "BMW M3 Competition 2024, culoare Alpine White, 510 CP, interior piele Merino.",
            imageUrl: "https://media.ed.edmunds-media.com/bmw/m3/2024/oem/2024_bmw_m3_sedan_competition_fq_oem_1_1600.jpg",
            ticketPrice: 50,
            availableTickets: 1000,
            endDate: "2024-04-15"
        },
        {
            id: 2,
            title: "Audi RS6 Avant 2024",
            description: "Audi RS6 Avant 2024, culoare Nardo Grey, 600 CP, pachet carbon.",
            imageUrl: "https://cdn.motor1.com/images/mgl/KqQjM/s3/2024-audi-rs6-avant-performance.jpg",
            ticketPrice: 75,
            availableTickets: 800,
            endDate: "2024-04-30"
        }
    ]
};

// Interceptor pentru adăugarea token-ului de autentificare
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Servicii pentru autentificare
export const authService = {
    login: async (email, password) => {
        try {
            const response = await api.post('/auth/login', { email, password });
            if (response.data.token) {
                localStorage.setItem('token', response.data.token);
            }
            return response.data;
        } catch (error) {
            throw new Error('Autentificare eșuată. Verificați credențialele.');
        }
    },
    logout: () => {
        localStorage.removeItem('token');
    },
    register: async (userData) => {
        try {
            const response = await api.post('/auth/register', userData);
            return response.data;
        } catch (error) {
            throw new Error('Înregistrare eșuată. Încercați din nou.');
        }
    }
};

// Servicii pentru rafle
export const raffleService = {
    getAllRaffles: async () => {
        try {
            // În mod normal, am face un apel API aici
            // return (await api.get('/raffles')).data;

            // Pentru dezvoltare, returnăm datele de test
            return TEST_DATA.raffles;
        } catch (error) {
            console.error('Error fetching raffles:', error);
            // Pentru dezvoltare, returnăm datele de test chiar și în caz de eroare
            return TEST_DATA.raffles;
        }
    },
    getRaffleById: async (id) => {
        try {
            // return (await api.get(`/raffles/${id}`)).data;
            return TEST_DATA.raffles.find(raffle => raffle.id === id);
        } catch (error) {
            throw new Error('Nu s-a putut găsi rafla specificată.');
        }
    },
    createRaffle: async (raffleData) => {
        try {
            const response = await api.post('/raffles', raffleData);
            return response.data;
        } catch (error) {
            throw new Error('Nu s-a putut crea rafla. Încercați din nou.');
        }
    },
    updateRaffle: async (id, raffleData) => {
        try {
            const response = await api.put(`/raffles/${id}`, raffleData);
            return response.data;
        } catch (error) {
            throw new Error('Nu s-a putut actualiza rafla.');
        }
    },
    deleteRaffle: async (id) => {
        try {
            const response = await api.delete(`/raffles/${id}`);
            return response.data;
        } catch (error) {
            throw new Error('Nu s-a putut șterge rafla.');
        }
    }
};

export default api; 