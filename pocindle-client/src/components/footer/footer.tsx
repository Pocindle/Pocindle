import React from 'react';
import { Link } from 'react-router-dom';
import './footer.scss';

const Footer: React.FC = () => {
  return (
    <div className="footer">
      <Link to="/about" className="footer__item">
        About
      </Link>
    </div>
  );
};

export default Footer;
