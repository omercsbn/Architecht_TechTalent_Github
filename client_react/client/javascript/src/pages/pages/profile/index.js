import React, { useState, useEffect } from 'react';
import { Typography, TextField, Button, Grid, Paper } from '@mui/material';
import { getUserProfile, updateUserProfile } from '../../../service/User';

const ProfilePage = () => {
  const [userProfile, setUserProfile] = useState({
    name: '',
    email: '',
    address: '',
    phone: '',
    balance: 0,
    surname: '',
    role: '',
  });

  useEffect(() => {
    const fetchUserProfile = async () => {
      try {
        const userId = localStorage.getItem('userId');
        const profileData = await getUserProfile(userId);
        setUserProfile(profileData);
      } catch (error) {
        console.error('Error fetching user profile:', error);
      }
    };

    fetchUserProfile();
  }, []);

  const handleChange = (e) => {
    setUserProfile({
      ...userProfile,
      [e.target.name]: e.target.value,
    });
  };

  const handleSave = async () => {
    try {
      const userId = localStorage.getItem('userId');
      await updateUserProfile(userId,userProfile);
      console.log('User profile updated successfully');
    } catch (error) {
      console.error('Error updating user profile:', error);
    }
  };

  return (
    <div style={{ padding: '20px' }}>
      <Typography variant="h4" gutterBottom>
        User Profile
      </Typography>
      <Paper style={{ padding: '20px', marginBottom: '20px' }}>
        <Grid container spacing={2}>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="Name"
              name="name"
              value={userProfile.name}
              onChange={handleChange}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="Surname"
              name="surname"
              value={userProfile.surname}
              onChange={handleChange}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Email"
              name="email"
              value={userProfile.email}
              onChange={handleChange}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Address"
              name="address"
              value={userProfile.address}
              onChange={handleChange}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="Phone"
              name="phone"
              value={userProfile.phone}
              onChange={handleChange}
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="Balance"
              name="balance"
              type="number"
              value={userProfile.balance}
              onChange={handleChange}
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Role"
              name="role"
              value={userProfile.role}
              onChange={handleChange}
            />
          </Grid>
        </Grid>
        <Button variant="contained" color="primary" onClick={handleSave}>
          Save
        </Button>
      </Paper>
    </div>
  );
};

export default ProfilePage;
