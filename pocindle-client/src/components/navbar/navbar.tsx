import React from 'react';
import { Link } from 'react-router-dom';
import useAppContext from '../../hooks/useAppContext';
import './navbar.scss';

const Navbar: React.FC<{ onLogOut: () => void }> = ({ onLogOut }) => {
  const { language } = useAppContext();

  const handleLogOut = () => {
    onLogOut();
  };

  return (
    <div className="navbar navbar__wrapper">
      <Link to="/" className="navbar__item navbar__link">
        Pocindle
      </Link>
      <Link to="/articles" className="navbar__item navbar__link">
        {language.navbar.articles}
      </Link>
      <div className="navbar__item" onClick={handleLogOut}>
        {language.navbar.logOut}
      </div>
    </div>
  );
};

export default Navbar;
