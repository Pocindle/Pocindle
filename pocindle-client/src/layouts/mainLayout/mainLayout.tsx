import React from 'react';
import { Navbar, Footer } from '../../components';
import './mainLayout.scss';

const MainLayout: React.FC<{ onLogOut: () => void }> = ({
  onLogOut,
  children,
}) => {
  return (
    <div className="main-layout main-layout__wrapper">
      <header className="header">
        <Navbar onLogOut={onLogOut} />
      </header>
      <div className="content">{children}</div>
      <Footer />
    </div>
  );
};

export default MainLayout;
