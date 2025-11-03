import React from 'react';
import { Container, Navbar } from 'reactstrap';
import TeamsTab from './components/TeamsTab';
import DriversTab from './components/DriversTab';
import LapTimesTab from './components/LapTimesTab';

function App() {
  return (
    <div className="App">
      <Navbar color="dark" dark expand="md" className="mb-4">
        <Container>
          <span className="navbar-brand">🏎️ SpeedFest</span>
          <span className="navbar-text">Formula 1 Racing Data Management</span>
        </Container>
      </Navbar>
      
      <Container>
        <nav>
          <div className="nav nav-tabs" id="nav-tab" role="tablist">
            <button 
              className="nav-link active" 
              id="nav-teams-tab" 
              data-bs-toggle="tab" 
              data-bs-target="#nav-teams" 
              type="button" 
              role="tab"
            >
              Racing Teams
            </button>
            <button 
              className="nav-link" 
              id="nav-drivers-tab" 
              data-bs-toggle="tab" 
              data-bs-target="#nav-drivers" 
              type="button" 
              role="tab"
            >
              Drivers
            </button>
            <button 
              className="nav-link" 
              id="nav-laptimes-tab" 
              data-bs-toggle="tab" 
              data-bs-target="#nav-laptimes" 
              type="button" 
              role="tab"
            >
              Lap Times
            </button>
          </div>
        </nav>
        
        <div className="tab-content" id="nav-tabContent">
          <div className="tab-pane fade show active" id="nav-teams" role="tabpanel">
            <TeamsTab />
          </div>
          <div className="tab-pane fade" id="nav-drivers" role="tabpanel">
            <DriversTab />
          </div>
          <div className="tab-pane fade" id="nav-laptimes" role="tabpanel">
            <LapTimesTab />
          </div>
        </div>
      </Container>
    </div>
  );
}

export default App;