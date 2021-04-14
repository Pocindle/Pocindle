import React from 'react';
import { Link } from 'react-router-dom';
import './footer.scss';

const Footer: React.FC = () => {
  return (
    <div className="footer">
      <div className="footer__top">
        <div className="footer__theme">
          <span className="footer__item">Site Theme:</span>
          <div className="footer__theme-options">
            <span className="footer__item footer__option footer__option_active">
              Dark
            </span>
            <span className="footer__item footer__option">Light</span>
          </div>
        </div>
        <div className="footer__language">
          <span className="footer__item">Language:</span>
          <div className="footer__language-options">
            <span className="footer__item footer__option footer__option_active">
              Eng
            </span>
            <span className="footer__item footer__option">Ru</span>
          </div>
        </div>
        <Link to="/about" className="footer__item">
          About
        </Link>
      </div>
      <div className="footer__bottom">
        <div className="footer__info footer__item">2021 Pocindle</div>
      </div>
    </div>
  );
};

export default Footer;
