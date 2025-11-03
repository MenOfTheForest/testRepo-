const API_BASE_URL = process.env.NODE_ENV === 'development' 
  ? 'http://localhost:5000/api' 
  : '/api';

class ApiService {
  async request(endpoint, options = {}) {
    const url = `${API_BASE_URL}${endpoint}`;
    const config = {
      headers: {
        'Content-Type': 'application/json',
      },
      ...options,
    };

    try {
      const response = await fetch(url, config);
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      return await response.json();
    } catch (error) {
      console.error('API request failed:', error);
      throw error;
    }
  }

  // Racing Teams
  async getTeams() {
    return this.request('/racingteams');
  }

  async getTeam(id) {
    return this.request(`/racingteams/${id}`);
  }

  async createTeam(team) {
    return this.request('/racingteams', {
      method: 'POST',
      body: JSON.stringify(team),
    });
  }

  async updateTeam(id, team) {
    return this.request(`/racingteams/${id}`, {
      method: 'PUT',
      body: JSON.stringify(team),
    });
  }

  async deleteTeam(id) {
    return this.request(`/racingteams/${id}`, {
      method: 'DELETE',
    });
  }

  // Racing Drivers
  async getDrivers() {
    return this.request('/racingdrivers');
  }

  async getDriver(id) {
    return this.request(`/racingdrivers/${id}`);
  }

  async getDriversByTeam(teamId) {
    return this.request(`/racingdrivers/team/${teamId}`);
  }

  async createDriver(driver) {
    return this.request('/racingdrivers', {
      method: 'POST',
      body: JSON.stringify(driver),
    });
  }

  async updateDriver(id, driver) {
    return this.request(`/racingdrivers/${id}`, {
      method: 'PUT',
      body: JSON.stringify(driver),
    });
  }

  async deleteDriver(id) {
    return this.request(`/racingdrivers/${id}`, {
      method: 'DELETE',
    });
  }

  // Lap Times
  async getLapTimes() {
    return this.request('/laptimes');
  }

  async getLapTime(id) {
    return this.request(`/laptimes/${id}`);
  }

  async getLapTimesByDriver(driverId) {
    return this.request(`/laptimes/driver/${driverId}`);
  }

  async getFastestLapTimes(count = 10) {
    return this.request(`/laptimes/fastest?count=${count}`);
  }

  async createLapTime(lapTime) {
    return this.request('/laptimes', {
      method: 'POST',
      body: JSON.stringify(lapTime),
    });
  }

  async updateLapTime(id, lapTime) {
    return this.request(`/laptimes/${id}`, {
      method: 'PUT',
      body: JSON.stringify(lapTime),
    });
  }

  async deleteLapTime(id) {
    return this.request(`/laptimes/${id}`, {
      method: 'DELETE',
    });
  }
}

export default new ApiService();