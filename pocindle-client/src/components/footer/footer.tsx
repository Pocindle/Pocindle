import React from 'react';
import { Link } from 'react-router-dom';
import useAppContext from '../../hooks/useAppContext';
import './footer.scss';

const Footer: React.FC = () => {
  const { language, switchLanguage } = useAppContext();

  return (
    <footer className="footer">
      <div className="footer__wrapper">
        <div className="footer__top">
          <div className="footer__switcher language-switcher">
            <span className="language-switcher__title">
              {language.footer.siteLanguage}
            </span>
            <ul className="language-switcher__options">
              <span
                className={`language-switcher__option option ${
                  language.name === 'ru' ? 'option_active' : ''
                }`}
                onClick={() => switchLanguage('ru')}
              >
                Ru
              </span>
              <span
                className={`language-switcher__option option ${
                  language.name === 'eng' ? 'option_active' : ''
                }`}
                onClick={() => switchLanguage('eng')}
              >
                Eng
              </span>
            </ul>
          </div>
          <Link to="/about" className="footer__link">
            {language.footer.about}
          </Link>
        </div>
        <div className="footer__bottom">
          <span className="footer__copyright">2021 Pocindle</span>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
