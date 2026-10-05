namespace Sengsara.Freepbx.Services;

/// <summary>
/// Reusable GraphQL selection sets for FreePBX object types.
/// </summary>
internal static class GraphQLFields
{
    public const string CoreUser = @"
        id
        extension
        name
        password
        voicemail
        ringtimer
        noanswer
        recording
        outboundCid
        sipname
        extPassword
        noanswerCid
        busyCid
        chanunavailCid
        noanswerDestination
        busyDestination
        chanunavailDestination
        mohclass
        callwaiting
        recording_in_external
        recording_out_external
        recording_in_internal
        recording_out_internal
        recording_ondemand
        recording_priority
        callforward_unconditional
        callforward_busy
        callforward_all
        callforward_ringtimer
        donotdisturb";

    public const string CoreDevice = @"
        id
        deviceId
        tech
        dial
        devicetype
        description
        emergencyCid
        user { " + CoreUser + @" }";

    public const string Extension = @"
        id
        extensionId
        tech
        user { " + CoreUser + @" }
        coreDevice {
            id
            deviceId
            tech
            dial
            devicetype
            description
            emergencyCid
        }";

    public const string RingGroup = @"
        id
        groupNumber
        description
        groupList
        groupTime
        groupPrefix
        needConf
        overrideRingerVolume
        changecid
        fixedcid
        callRecording
        pickupCall
        callProgress
        answeredElseWhere
        ignoreCallForward
        ignoreCallWait
        alertInfo
        receiverMessageConfirmCall
        receiverMessage
        postAnswer
        callerMessage
        ringingMusic
        strategy";

    public const string InboundRoute = @"
        id
        extension
        cidnum
        description
        privacyman
        alertinfo
        ringing
        mohclass
        grppre
        delay_answer
        pricid
        pmmaxretries
        pmminlength
        reversal
        rvolume
        fanswer
        destinationConnection";

    public const string Recording = @"
        id
        name
        description
        fcode
        fcode_pass
        language
        playback
        languages";

    public const string MusicOnHold = @"
        id
        category
        type
        random
        application
        format";

    public const string VoiceMail = @"
        id
        status
        message
        context
        password
        name
        email
        pager
        attach
        saycid
        envelope
        delete";

    public const string FollowMe = @"
        id
        status
        message
        enabled
        extensionId
        strategy
        ringTime
        followMePrefix
        followMeList
        callerMessage
        noAnswerDestination
        alertInfo
        confirmCalls
        receiverMessageConfirmCall
        receiverMessageTooLate
        ringingMusic
        initialRingTime
        voicemail
        enableCalendar
        matchCalendar
        calendar
        calendarGroup
        overrideRingerVolume
        externalCallerIdMode
        fixedCallerId";

    public const string Cdr = @"
        id
        uniqueid
        calldate
        timestamp
        clid
        src
        dst
        dcontext
        channel
        dstchannel
        lastapp
        lastdata
        duration
        billsec
        disposition
        amaflags
        accountcode
        userfield
        did
        recordingfile
        cnum
        outbound_cnum
        outbound_cnam
        dst_cnam
        linkedid
        peeraccount
        sequence";
}
