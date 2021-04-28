import React from 'react';
import { Navbar, Footer } from '../../components';
import './mainLayout.scss';
import useAppContext from '../../hooks/useAppContext';

const MainLayout: React.FC = ({ children }) => {
  const { language } = useAppContext();

  return (
    <div className="main-layout main-layout__wrapper">
      <header className="header">
        <Navbar
          articles={language.navbar.articles}
          logOut={language.navbar.logOut}
        />
      </header>
      <div className="content">{children}</div>
      <Footer
        about={language.footer.about}
        siteTheme={language.footer.siteTheme}
        language={language.footer.language}
      />
    </div>
  );
};

export default MainLayout;
