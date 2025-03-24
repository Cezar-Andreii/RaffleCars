import React from 'react';
import './HomePage.css';

const HomePage = () => {
    return (
        <div className="home-page">
            <div className="hero-section">
                <h1>Bine ați venit la RaffleCars</h1>
                <p>Platforma ta pentru câștigarea mașinilor de vis prin tombole transparente și sigure.</p>
            </div>
            <div className="features-section">
                <div className="feature">
                    <h3>Transparent</h3>
                    <p>Toate tragerile sunt realizate în mod transparent și verificabil.</p>
                </div>
                <div className="feature">
                    <h3>Sigur</h3>
                    <p>Plăți securizate și sistem de verificare a câștigătorilor.</p>
                </div>
                <div className="feature">
                    <h3>Accesibil</h3>
                    <p>Prețuri accesibile pentru șansa de a câștiga mașina visurilor tale.</p>
                </div>
            </div>
        </div>
    );
};

export default HomePage; 