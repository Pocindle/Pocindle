import React from 'react';
import { Link } from 'react-router-dom';
import './footer.scss';

const Footer: React.FC<{
  about?: string;
  siteTheme?: string;
  language?: string;
}> = ({
  about = 'About',
  siteTheme = 'Site Theme:',
  language = 'Language:',
}) => {
  return (
    <footer className="footer footer__wrapper">
      <div className="footer__top">
        <div className="footer__switcher theme-switcher">
          <span className="theme-switcher__title">{siteTheme}</span>
          <ul className="theme-switcher__options">
            <span className="theme-switcher__option option option_active">
              Light
            </span>
            <span className="theme-switcher__option option">Dark</span>
          </ul>
        </div>
        <div className="footer__switcher language-switcher">
          <span className="language-switcher__title">{language}</span>
          <ul className="language-switcher__options">
            <span className="language-switcher__option option option_active">
              Ru
            </span>
            <span className="language-switcher__option option">Eng</span>
          </ul>
        </div>
        <Link to="/about" className="footer__link">
          {about}
        </Link>
      </div>
      <div className="footer__bottom">
        <span className="footer__copyright">2021 Pocindle</span>
      </div>
    </footer>
  );
};

export default Footer;
