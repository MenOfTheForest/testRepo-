import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, ModalHeader, ModalBody, Form, FormGroup, Label, Input, Alert } from 'reactstrap';
import ApiService from '../services/apiService';

function DriversTab() {
  const [drivers, setDrivers] = useState([]);
  const [teams, setTeams] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [modal, setModal] = useState(false);
  const [editingDriver, setEditingDriver] = useState(null);
  const [formData, setFormData] = useState({
    name: '',
    driverNumber: '',
    racingTeamId: ''
  });

  const toggleModal = () => setModal(!modal);

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [driversData, teamsData] = await Promise.all([
        ApiService.getDrivers(),
        ApiService.getTeams()
      ]);
      setDrivers(driversData);
      setTeams(teamsData);
      setError(null);
    } catch (err) {
      setError('Failed to load data: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (driver) => {
    setEditingDriver(driver);
    setFormData({
      name: driver.name,
      driverNumber: driver.driverNumber.toString(),
      racingTeamId: driver.racingTeamId.toString()
    });
    toggleModal();
  };

  const handleAdd = () => {
    setEditingDriver(null);
    setFormData({
      name: '',
      driverNumber: '',
      racingTeamId: ''
    });
    toggleModal();
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const data = {
        ...formData,
        driverNumber: parseInt(formData.driverNumber),
        racingTeamId: parseInt(formData.racingTeamId)
      };
      
      if (editingDriver) {
        await ApiService.updateDriver(editingDriver.racingDriverId, data);
      } else {
        await ApiService.createDriver(data);
      }
      toggleModal();
      loadData();
    } catch (err) {
      setError('Failed to save driver: ' + err.message);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this driver?')) {
      try {
        await ApiService.deleteDriver(id);
        loadData();
      } catch (err) {
        setError('Failed to delete driver: ' + err.message);
      }
    }
  };

  if (loading) return <div className="text-center">Loading drivers...</div>;

  return (
    <div className="mt-3">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2>Racing Drivers</h2>
        <Button color="primary" onClick={handleAdd}>Add Driver</Button>
      </div>

      {error && <Alert color="danger">{error}</Alert>}

      <Table striped responsive>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Number</th>
            <th>Team</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {drivers.map(driver => (
            <tr key={driver.racingDriverId}>
              <td>{driver.racingDriverId}</td>
              <td>{driver.name}</td>
              <td>{driver.driverNumber}</td>
              <td>{driver.teamName}</td>
              <td>
                <Button size="sm" color="info" className="me-2" onClick={() => handleEdit(driver)}>
                  Edit
                </Button>
                <Button size="sm" color="danger" onClick={() => handleDelete(driver.racingDriverId)}>
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      <Modal isOpen={modal} toggle={toggleModal}>
        <ModalHeader toggle={toggleModal}>
          {editingDriver ? 'Edit Driver' : 'Add Driver'}
        </ModalHeader>
        <ModalBody>
          <Form onSubmit={handleSubmit}>
            <FormGroup>
              <Label for="name">Driver Name</Label>
              <Input
                type="text"
                name="name"
                id="name"
                value={formData.name}
                onChange={(e) => setFormData({...formData, name: e.target.value})}
                required
              />
            </FormGroup>
            <FormGroup>
              <Label for="driverNumber">Driver Number</Label>
              <Input
                type="number"
                name="driverNumber"
                id="driverNumber"
                min="1"
                max="99"
                value={formData.driverNumber}
                onChange={(e) => setFormData({...formData, driverNumber: e.target.value})}
                required
              />
            </FormGroup>
            <FormGroup>
              <Label for="racingTeamId">Team</Label>
              <Input
                type="select"
                name="racingTeamId"
                id="racingTeamId"
                value={formData.racingTeamId}
                onChange={(e) => setFormData({...formData, racingTeamId: e.target.value})}
                required
              >
                <option value="">Select a team</option>
                {teams.map(team => (
                  <option key={team.racingTeamId} value={team.racingTeamId}>
                    {team.name}
                  </option>
                ))}
              </Input>
            </FormGroup>
            <div className="d-flex justify-content-end">
              <Button type="button" color="secondary" className="me-2" onClick={toggleModal}>
                Cancel
              </Button>
              <Button type="submit" color="primary">
                {editingDriver ? 'Update' : 'Add'}
              </Button>
            </div>
          </Form>
        </ModalBody>
      </Modal>
    </div>
  );
}

export default DriversTab;