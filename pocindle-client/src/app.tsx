import React from 'react';
import AppRoute from './routes/appRoute';
import { ThemeProvider, LanguageProvider } from './providers';

const App: React.FC = () => {
  return (
    <React.Fragment>
      <LanguageProvider>
        <ThemeProvider>
          <AppRoute />
        </ThemeProvider>
      </LanguageProvider>
    </React.Fragment>
  );
};

export default App;
