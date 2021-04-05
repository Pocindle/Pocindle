import React from 'react';
import { Navbar, Footer } from '../../components';
import './mainLayout.scss';

const MainLayout: React.FC = ({ children }) => {
  return (
    <div className="main-layout">
      <div className="main-layout__header">
        <Navbar />
      </div>
      <div className="main-layout__page-content">{children}</div>
      <div className="main-layout__footer">
        <Footer />
      </div>
    </div>
  );
};

export default MainLayout;
