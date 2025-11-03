import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, ModalHeader, ModalBody, Form, FormGroup, Label, Input, Alert } from 'reactstrap';
import ApiService from '../services/apiService';

function TeamsTab() {
  const [teams, setTeams] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [modal, setModal] = useState(false);
  const [editingTeam, setEditingTeam] = useState(null);
  const [formData, setFormData] = useState({
    name: '',
    teamPrincipal: ''
  });

  const toggleModal = () => setModal(!modal);

  useEffect(() => {
    loadTeams();
  }, []);

  const loadTeams = async () => {
    try {
      setLoading(true);
      const data = await ApiService.getTeams();
      setTeams(data);
      setError(null);
    } catch (err) {
      setError('Failed to load teams: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (team) => {
    setEditingTeam(team);
    setFormData({
      name: team.name,
      teamPrincipal: team.teamPrincipal
    });
    toggleModal();
  };

  const handleAdd = () => {
    setEditingTeam(null);
    setFormData({
      name: '',
      teamPrincipal: ''
    });
    toggleModal();
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingTeam) {
        await ApiService.updateTeam(editingTeam.racingTeamId, formData);
      } else {
        await ApiService.createTeam(formData);
      }
      toggleModal();
      loadTeams();
    } catch (err) {
      setError('Failed to save team: ' + err.message);
    }
  };

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this team?')) {
      try {
        await ApiService.deleteTeam(id);
        loadTeams();
      } catch (err) {
        setError('Failed to delete team: ' + err.message);
      }
    }
  };

  if (loading) return <div className="text-center">Loading teams...</div>;

  return (
    <div className="mt-3">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2>Racing Teams</h2>
        <Button color="primary" onClick={handleAdd}>Add Team</Button>
      </div>

      {error && <Alert color="danger">{error}</Alert>}

      <Table striped responsive>
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Team Principal</th>
            <th>Drivers Count</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {teams.map(team => (
            <tr key={team.racingTeamId}>
              <td>{team.racingTeamId}</td>
              <td>{team.name}</td>
              <td>{team.teamPrincipal}</td>
              <td>{team.drivers.length}</td>
              <td>
                <Button size="sm" color="info" className="me-2" onClick={() => handleEdit(team)}>
                  Edit
                </Button>
                <Button size="sm" color="danger" onClick={() => handleDelete(team.racingTeamId)}>
                  Delete
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      <Modal isOpen={modal} toggle={toggleModal}>
        <ModalHeader toggle={toggleModal}>
          {editingTeam ? 'Edit Team' : 'Add Team'}
        </ModalHeader>
        <ModalBody>
          <Form onSubmit={handleSubmit}>
            <FormGroup>
              <Label for="name">Team Name</Label>
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
              <Label for="teamPrincipal">Team Principal</Label>
              <Input
                type="text"
                name="teamPrincipal"
                id="teamPrincipal"
                value={formData.teamPrincipal}
                onChange={(e) => setFormData({...formData, teamPrincipal: e.target.value})}
                required
              />
            </FormGroup>
            <div className="d-flex justify-content-end">
              <Button type="button" color="secondary" className="me-2" onClick={toggleModal}>
                Cancel
              </Button>
              <Button type="submit" color="primary">
                {editingTeam ? 'Update' : 'Add'}
              </Button>
            </div>
          </Form>
        </ModalBody>
      </Modal>
    </div>
  );
}

export default TeamsTab;