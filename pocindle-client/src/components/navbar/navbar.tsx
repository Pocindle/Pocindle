import React from 'react';
import { Link } from 'react-router-dom';
import './navbar.scss';

const Navbar: React.FC = () => {
  return (
    <div className="navbar">
      <Link to="/" className="navbar__item">
        Pocindle
      </Link>
      <Link to="/articles" className="navbar__item">
        Articles
      </Link>
      <div className="navbar__item">Log out</div>
    </div>
  );
};

export default Navbar;
