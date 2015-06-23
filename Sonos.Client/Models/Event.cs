﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sonos.Client.Models
{

    [XmlRoot(ElementName = "TransportState")]
    public class TransportState
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
        public bool Playing
        {
            get
            {
                if (Val == null)
                    return false;
                return Val == "PLAYING" ? true : false;
            }
        }
    }

    [XmlRoot(ElementName = "CurrentPlayMode")]
    public class CurrentPlayMode
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "CurrentCrossfadeMode")]
    public class CurrentCrossfadeMode
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "NumberOfTracks")]
    public class NumberOfTracks
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "CurrentTrack")]
    public class CurrentTrack
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "CurrentSection")]
    public class CurrentSection
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "CurrentTrackURI")]
    public class CurrentTrackURI
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "CurrentTrackDuration")]
    public class CurrentTrackDuration
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.Parse(Val);
            }
        }
    }

    [XmlRoot(ElementName = "CurrentTrackMetaData")]
    public class CurrentTrackMetaData
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
        public TrackMeta TrackMeta { get; set; }
    }

    [XmlRoot(ElementName = "NextTrackURI")]
    public class NextTrackURI
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "NextTrackMetaData")]
    public class NextTrackMetaData
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
        public TrackMeta TrackMeta { get; set; }
    }

    [XmlRoot(ElementName = "EnqueuedTransportURI")]
    public class EnqueuedTransportURI
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "EnqueuedTransportURIMetaData")]
    public class EnqueuedTransportURIMetaData
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "PlaybackStorageMedium")]
    public class PlaybackStorageMedium
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "AVTransportURI")]
    public class AVTransportURI
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "AVTransportURIMetaData")]
    public class AVTransportURIMetaData
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "NextAVTransportURI")]
    public class NextAVTransportURI
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "NextAVTransportURIMetaData")]
    public class NextAVTransportURIMetaData
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "CurrentTransportActions")]
    public class CurrentTransportActions
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "CurrentValidPlayModes")]
    public class CurrentValidPlayModes
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "TransportStatus")]
    public class TransportStatus
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "SleepTimerGeneration")]
    public class SleepTimerGeneration
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "AlarmRunning")]
    public class AlarmRunning
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "SnoozeRunning")]
    public class SnoozeRunning
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "RestartPending")]
    public class RestartPending
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "TransportPlaySpeed")]
    public class TransportPlaySpeed
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "CurrentMediaDuration")]
    public class CurrentMediaDuration
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "RecordStorageMedium")]
    public class RecordStorageMedium
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "PossiblePlaybackStorageMedia")]
    public class PossiblePlaybackStorageMedia
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "PossibleRecordStorageMedia")]
    public class PossibleRecordStorageMedia
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "RecordMediumWriteStatus")]
    public class RecordMediumWriteStatus
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "CurrentRecordQualityMode")]
    public class CurrentRecordQualityMode
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "PossibleRecordQualityModes")]
    public class PossibleRecordQualityModes
    {
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "InstanceID")]
    public class InstanceID
    {
        [XmlElement(ElementName = "TransportState")]
        public TransportState TransportState { get; set; }
        [XmlElement(ElementName = "CurrentPlayMode")]
        public CurrentPlayMode CurrentPlayMode { get; set; }
        [XmlElement(ElementName = "CurrentCrossfadeMode")]
        public CurrentCrossfadeMode CurrentCrossfadeMode { get; set; }
        [XmlElement(ElementName = "NumberOfTracks")]
        public NumberOfTracks NumberOfTracks { get; set; }
        [XmlElement(ElementName = "CurrentTrack")]
        public CurrentTrack CurrentTrack { get; set; }
        [XmlElement(ElementName = "CurrentSection")]
        public CurrentSection CurrentSection { get; set; }
        [XmlElement(ElementName = "CurrentTrackURI")]
        public CurrentTrackURI CurrentTrackURI { get; set; }
        [XmlElement(ElementName = "CurrentTrackDuration")]
        public CurrentTrackDuration CurrentTrackDuration { get; set; }
        [XmlElement(ElementName = "CurrentTrackMetaData")]
        public CurrentTrackMetaData CurrentTrackMetaData { get; set; }
        [XmlElement(ElementName = "NextTrackURI")]
        public NextTrackURI NextTrackURI { get; set; }
        [XmlElement(ElementName = "NextTrackMetaData")]
        public NextTrackMetaData NextTrackMetaData { get; set; }
        [XmlElement(ElementName = "EnqueuedTransportURI")]
        public EnqueuedTransportURI EnqueuedTransportURI { get; set; }
        [XmlElement(ElementName = "EnqueuedTransportURIMetaData")]
        public EnqueuedTransportURIMetaData EnqueuedTransportURIMetaData { get; set; }
        [XmlElement(ElementName = "PlaybackStorageMedium")]
        public PlaybackStorageMedium PlaybackStorageMedium { get; set; }
        [XmlElement(ElementName = "AVTransportURI")]
        public AVTransportURI AVTransportURI { get; set; }
        [XmlElement(ElementName = "AVTransportURIMetaData")]
        public AVTransportURIMetaData AVTransportURIMetaData { get; set; }
        [XmlElement(ElementName = "NextAVTransportURI")]
        public NextAVTransportURI NextAVTransportURI { get; set; }
        [XmlElement(ElementName = "NextAVTransportURIMetaData")]
        public NextAVTransportURIMetaData NextAVTransportURIMetaData { get; set; }
        [XmlElement(ElementName = "CurrentTransportActions")]
        public CurrentTransportActions CurrentTransportActions { get; set; }
        [XmlElement(ElementName = "CurrentValidPlayModes")]
        public CurrentValidPlayModes CurrentValidPlayModes { get; set; }
        [XmlElement(ElementName = "TransportStatus")]
        public TransportStatus TransportStatus { get; set; }
        [XmlElement(ElementName = "SleepTimerGeneration")]
        public SleepTimerGeneration SleepTimerGeneration { get; set; }
        [XmlElement(ElementName = "AlarmRunning")]
        public AlarmRunning AlarmRunning { get; set; }
        [XmlElement(ElementName = "SnoozeRunning")]
        public SnoozeRunning SnoozeRunning { get; set; }
        [XmlElement(ElementName = "RestartPending")]
        public RestartPending RestartPending { get; set; }
        [XmlElement(ElementName = "TransportPlaySpeed")]
        public TransportPlaySpeed TransportPlaySpeed { get; set; }
        [XmlElement(ElementName = "CurrentMediaDuration")]
        public CurrentMediaDuration CurrentMediaDuration { get; set; }
        [XmlElement(ElementName = "RecordStorageMedium")]
        public RecordStorageMedium RecordStorageMedium { get; set; }
        [XmlElement(ElementName = "PossiblePlaybackStorageMedia")]
        public PossiblePlaybackStorageMedia PossiblePlaybackStorageMedia { get; set; }
        [XmlElement(ElementName = "PossibleRecordStorageMedia")]
        public PossibleRecordStorageMedia PossibleRecordStorageMedia { get; set; }
        [XmlElement(ElementName = "RecordMediumWriteStatus")]
        public RecordMediumWriteStatus RecordMediumWriteStatus { get; set; }
        [XmlElement(ElementName = "CurrentRecordQualityMode")]
        public CurrentRecordQualityMode CurrentRecordQualityMode { get; set; }
        [XmlElement(ElementName = "PossibleRecordQualityModes")]
        public PossibleRecordQualityModes PossibleRecordQualityModes { get; set; }
        [XmlAttribute(AttributeName = "val")]
        public string Val { get; set; }
    }

    [XmlRoot(ElementName = "Event")]
    public class Event
    {
        [XmlElement(ElementName = "InstanceID")]
        public InstanceID InstanceID { get; set; }
    }

}