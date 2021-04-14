import React from 'react';
import { Link } from 'react-router-dom';
import './navbar.scss';

const Navbar: React.FC<{ articles?: string; logOut?: string }> = ({
  articles = 'Articles',
  logOut = 'Log Out',
}) => {
  return (
    <div className="navbar">
      <Link to="/" className="navbar__item">
        Pocindle
      </Link>
      <Link to="/articles" className="navbar__item">
        {articles}
      </Link>
      <div className="navbar__item">{logOut}</div>
    </div>
  );
};

export default Navbar;
