import React, { useState, useEffect, useCallback } from 'react';
import { Table, Button, Modal, ModalHeader, ModalBody, Form, FormGroup, Label, Input, Alert, Badge } from 'reactstrap';
import ApiService from '../services/apiService';

function LapTimesTab() {
  const [lapTimes, setLapTimes] = useState([]);
  const [drivers, setDrivers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [modal, setModal] = useState(false);
  const [editingLapTime, setEditingLapTime] = useState(null);
  const [showFastest, setShowFastest] = useState(false);
  const [formData, setFormData] = useState({
    racingDriverId: '',
    startDateTime: '',
    sector1ElapsedTime: '',
    sector2ElapsedTime: '',
    sector3ElapsedTime: ''
  });

  const toggleModal = () => setModal(!modal);

  const loadData = useCallback(async () => {
    try {
      setLoading(true);
      const [lapTimesData, driversData] = await Promise.all([
        showFastest ? ApiService.getFastestLapTimes(20) : ApiService.getLapTimes(),
        ApiService.getDrivers()
      ]);
      setLapTimes(lapTimesData);
      setDrivers(driversData);
      setError(null);
    } catch (err) {
      setError('Failed to load data: ' + err.message);
    } finally {
      setLoading(false);
    }
  }, [showFastest]);

  useEffect(() => {
    loadData();
  }, [loadData]);

  const toggleFastest = () => {
    setShowFastest(!showFastest);
    loadData();
  };

  const formatTime = (seconds) => {
    if (!seconds) return 'N/A';
    return seconds.toFixed(3) + 's';
  };

  const formatDateTime = (dateTime) => {
    return new Date(dateTime).toLocaleString();
  };

  const handleEdit = (lapTime) => {
    setEditingLapTime(lapTime);
    setFormData({
      racingDriverId: lapTime.racingDriverId.toString(),
      startDateTime: new Date(lapTime.startDateTime).toISOString().slice(0, 16),
      sector1ElapsedTime: lapTime.sector1ElapsedTime?.toString() || '',
      sector2ElapsedTime: lapTime.sector2ElapsedTime?.toString() || '',
      sector3ElapsedTime: lapTime.sector3ElapsedTime?.toString() || ''
    });
    toggleModal();
  };

  const handleAdd = () => {
    setEditingLapTime(null);
    setFormData({
      racingDriverId: '',
      startDateTime: new Date().toISOString().slice(0, 16),
      sector1ElapsedTime: '',
      sector2ElapsedTime: '',
      sector3ElapsedTime: ''
    });
    toggleModal();
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const data = {
        racingDriverId: parseInt(formData.racingDriverId),
        startDateTime: new Date(formData.startDateTime).toISOString(),
        sector1ElapsedTime: formData.sector1ElapsedTime ? parseFloat(formData.sector1ElapsedTime) : null,
        sector2ElapsedTime: formData.sector2ElapsedTime ? parseFloat(formData.sector2ElapsedTime) : null,
        sector3ElapsedTime: formData.sector3ElapsedTime ? parseFloat(formData.sector3ElapsedTime) : null
      };
      
      if (editingLapTime) {
        await ApiService.updateLapTime(editingLapTime.lapTimeId, data);
      } else {
        await ApiService.createLapTime(data);
      }
      toggleModal();
      loadData();
    } catch (err) {
      setError('Failed to save lap time: ' + err.message);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this lap time?')) {
      try {
        await ApiService.deleteLapTime(id);
        loadData();
      } catch (err) {
        setError('Failed to delete lap time: ' + err.message);
      }
    }
  };

  if (loading) return <div className="text-center">Loading lap times...</div>;

  return (
    <div className="mt-3">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2>Lap Times</h2>
        <div>
          <Button 
            color={showFastest ? "success" : "outline-success"} 
            className="me-2"
            onClick={toggleFastest}
          >
            {showFastest ? "Show All" : "Show Fastest"}
          </Button>
          <Button color="primary" onClick={handleAdd}>Add Lap Time</Button>
        </div>
      </div>

      {error && <Alert color="danger">{error}</Alert>}

      <Table striped responsive>
        <thead>
          <tr>
            <th>ID</th>
            <th>Driver</th>
            <th>Team</th>
            <th>Start Time</th>
            <th>Sector 1</th>
            <th>Sector 2</th>
            <th>Sector 3</th>
            <th>Total Time</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {lapTimes.map((lapTime, index) => (
            <tr key={lapTime.lapTimeId}>
              <td>{lapTime.lapTimeId}</td>
              <td>{lapTime.driverName}</td>
              <td>{lapTime.teamName}</td>
              <td>{formatDateTime(lapTime.startDateTime)}</td>
              <td>{formatTime(lapTime.sector1ElapsedTime)}</td>
              <td>{formatTime(lapTime.sector2ElapsedTime)}</td>
              <td>{formatTime(lapTime.sector3ElapsedTime)}</td>
              <td>
                {lapTime.totalLapTime ? (
                  <>
                    {formatTime(lapTime.totalLapTime)}
                    {showFastest && index < 3 && (
                      <Badge color={index === 0 ? "warning" : index === 1 ? "secondary" : "dark"} className="ms-2">
                        {index === 0 ? "🥇" : index === 1 ? "🥈" : "🥉"}
                      </Badge>
                    )}
                  </>
                ) : 'Incomplete'}
              </td>
              <td>
                <Button size="sm" color="info" className="me-2" onClick={() => handleEdit(lapTime)}>
                  Edit
                </Button>
                <Button size="sm" color="danger" onClick={() => handleDelete(lapTime.lapTimeId)}>
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      <Modal isOpen={modal} toggle={toggleModal}>
        <ModalHeader toggle={toggleModal}>
          {editingLapTime ? 'Edit Lap Time' : 'Add Lap Time'}
        </ModalHeader>
        <ModalBody>
          <Form onSubmit={handleSubmit}>
            <FormGroup>
              <Label for="racingDriverId">Driver</Label>
              <Input
                type="select"
                name="racingDriverId"
                id="racingDriverId"
                value={formData.racingDriverId}
                onChange={(e) => setFormData({...formData, racingDriverId: e.target.value})}
                required
              >
                <option value="">Select a driver</option>
                {drivers.map(driver => (
                  <option key={driver.racingDriverId} value={driver.racingDriverId}>
                    {driver.name} (#{driver.driverNumber}) - {driver.teamName}
                  </option>
                ))}
              </Input>
            </FormGroup>
            <FormGroup>
              <Label for="startDateTime">Start Date & Time</Label>
              <Input
                type="datetime-local"
                name="startDateTime"
                id="startDateTime"
                value={formData.startDateTime}
                onChange={(e) => setFormData({...formData, startDateTime: e.target.value})}
                required
              />
            </FormGroup>
            <FormGroup>
              <Label for="sector1ElapsedTime">Sector 1 Time (seconds)</Label>
              <Input
                type="number"
                step="0.001"
                name="sector1ElapsedTime"
                id="sector1ElapsedTime"
                value={formData.sector1ElapsedTime}
                onChange={(e) => setFormData({...formData, sector1ElapsedTime: e.target.value})}
                placeholder="e.g., 30.123"
              />
            </FormGroup>
            <FormGroup>
              <Label for="sector2ElapsedTime">Sector 2 Time (seconds)</Label>
              <Input
                type="number"
                step="0.001"
                name="sector2ElapsedTime"
                id="sector2ElapsedTime"
                value={formData.sector2ElapsedTime}
                onChange={(e) => setFormData({...formData, sector2ElapsedTime: e.target.value})}
                placeholder="e.g., 28.456"
              />
            </FormGroup>
            <FormGroup>
              <Label for="sector3ElapsedTime">Sector 3 Time (seconds)</Label>
              <Input
                type="number"
                step="0.001"
                name="sector3ElapsedTime"
                id="sector3ElapsedTime"
                value={formData.sector3ElapsedTime}
                onChange={(e) => setFormData({...formData, sector3ElapsedTime: e.target.value})}
                placeholder="e.g., 32.789"
              />
            </FormGroup>
            <div className="d-flex justify-content-end">
              <Button type="button" color="secondary" className="me-2" onClick={toggleModal}>
                Cancel
              </Button>
              <Button type="submit" color="primary">
                {editingLapTime ? 'Update' : 'Add'}
              </Button>
            </div>
          </Form>
        </ModalBody>
      </Modal>
    </div>
  );
}

export default LapTimesTab;