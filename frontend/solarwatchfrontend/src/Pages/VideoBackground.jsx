import React from 'react';
import '../Styles/videobackground.css'; 
import videobackground from "../Styling-accesories/background.mp4"
import Layout from './Layout';
import NavigationBar from '../Components/NavigationBar';

    const VideoBackground = () => {
        return (
          <div className="video-background">
            <video autoPlay loop muted>
              <source src={videobackground} type="video/mp4" />
            </video>
           
            <div className="content">
            <Layout /> 
            </div>
          </div>
        );
      };

export default VideoBackground;